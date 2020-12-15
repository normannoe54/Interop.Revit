#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.IO;
using System.Reflection;
#endregion

namespace QAQCSAP
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventRun : IExternalEventHandler
    {
        /// <summary>
        /// Initialize form
        /// </summary>
        public QAQCForm qaqcform;

        /// <summary>
        /// Execute Run Event
        /// </summary>
        /// <param name="uiapp"></param>
        public void Execute(UIApplication uiapp)
        {
            //Define Document
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            #region Shared Parameter Source File
            //Get the executing path to locate the text file
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //Shared Parameter file name
            string sharedParameterFile = directoryName + "\\SharedParamSAP.txt";

            //Set shared parameter file to application
            app.SharedParametersFilename = sharedParameterFile;

            //Define the Definition File
            DefinitionFile SP_DefFile = app.OpenSharedParameterFile();

            //Define the Definition File Group
            DefinitionGroup QAQCgroup = SP_DefFile.Groups.get_Item("QAQC_ParameterGroup");

            #endregion

            #region List of All Parameter Names

            //Collect Structural Framing Parameters
            List<string> SFParamaterNames = InternalConstants.StructuralFramingParameters();

            //List of parameter names to create
            List<string> CreateParamaterNames = new List<string>();

            #endregion

            #region Label category sets (Structural Framing, Columns, VB)
            Category SFcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
            Category Colcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
            Category VBcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
            CategorySet SFcatset = app.Create.NewCategorySet();
            SFcatset.Insert(SFcat);            
            SFcatset.Insert(Colcat);           
            SFcatset.Insert(VBcat);
            #endregion

            #region Create Shared Parameters
            Parameters.CreateSharedParameters(SFParamaterNames, SFcatset, doc, app, QAQCgroup);

            #endregion

            #region Collect Revit Beam Elements [Revit]

            //Retrieve beams
            IList<Element> BeamElements = RevitCollectElement.SFElements(doc);

            //Initialize Beam Model
            List<BeamDataModel> RevitBeams = new List<BeamDataModel>();          

            //Iterate Beam Elements and set to data model
            foreach (Element BeamElement in BeamElements)
            {
                var Beam = new BeamDataModel
                {
                    x = CollectLocation.StructuralFramingMidpoint(BeamElement).X,
                    y = CollectLocation.StructuralFramingMidpoint(BeamElement).Y,
                    z = CollectLocation.StructuralFramingMidpoint(BeamElement).Z,
                    name = BeamElement.Name.ToUpper(),  
                    element = BeamElement,
                };

                RevitBeams.Add(Beam);
            }
              
            #endregion

            #region Collect Column Elements [Revit]

            //Retrieve Columns
            IList<Element> ColumnElements = RevitCollectElement.ColElements(doc);

            //Initialize Beam Model
            List<BeamDataModel> RevitColumns = new List<BeamDataModel>();

            foreach (Element ColumnElement in ColumnElements)
            {
                var Column = new BeamDataModel
                {
                    x = CollectLocation.StructuralColumnMidpoint(ColumnElement).X,
                    y = CollectLocation.StructuralColumnMidpoint(ColumnElement).Y,
                    z = CollectLocation.StructuralColumnMidpoint(ColumnElement).Z,
                    name = ColumnElement.Name.ToUpper(),      
                    element=ColumnElement,
                };

                RevitColumns.Add(Column);
            }
               
            #endregion

            #region Collect Structural Braces Elements [Revit]

            //Retrieve VB
            IList<Element> VBElements = RevitCollectElement.VBElements(doc);

            //Initialize VB Model
            List<BeamDataModel> RevitVBs = new List<BeamDataModel>();

            foreach (Element VBElement in VBElements)
            {
                var VB = new BeamDataModel
                {
                    x = CollectLocation.StructuralFramingMidpoint(VBElement).X,
                    y = CollectLocation.StructuralFramingMidpoint(VBElement).Y,
                    z = CollectLocation.StructuralFramingMidpoint(VBElement).Z,
                    name = VBElement.Name.ToUpper(),
                    element = VBElement,

                };

                RevitVBs.Add(VB);
            }
            #endregion

            #region Concatenate List of Revit Members
            //Concatenate Lists for BeamDataModel
            List<BeamDataModel> RevitMembers = new List<BeamDataModel>();
            RevitMembers = RevitBeams.Concat(RevitColumns).Concat(RevitVBs).ToList();
            #endregion

            #region Collect Inputs from Userform
            //Retrieve inputs from Userform
            string FilenameUser = qaqcform.FilenameText.Text;
            string ElementIDUser = qaqcform.ElementIDText.Text;
            string SAPIDUser = qaqcform.SAPIDText.Text;
            string RotationUser = qaqcform.RotText.Text;
            string ToleranceUser = qaqcform.ToleranceText.Text;
            bool ClearToggleUser = qaqcform.ClearToggle.Checked;
            #endregion

            #region Catch for $2k file missing
            string USRCheck = FilenameUser.Replace(@".sdb", ".$2k");
            bool CheckExists = File.Exists(USRCheck);

            if (CheckExists == false)
            {
                Message.Display("$2k file could not be found", WindowType.Warning);
                return;
            }
            #endregion

            #region Collect SAP Members
            //Initialize method

            CollectInfo SAPCollection = new CollectInfo();

            //Get the beams
            List<BeamDataModel> SAPBeams = SAPCollection.GetBeams(FilenameUser.Replace(@".sdb", ".$2k"));

            #endregion

            #region Rotate the SAP Members

            //Rotation
            //Convert string to int
            double Rotationdouble = Convert.ToDouble(RotationUser);

            //Convert Angle to radians
            double angle = Rotationdouble * (Math.PI / 180);

            //Translate the Beams in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (BeamDataModel beam in SAPBeams)
            {
                //Rotate the beam geometry
                double rotatedxbeam = beam.x * Math.Cos(angle) - beam.y * Math.Sin(angle);
                double rotatedybeam = beam.x * Math.Sin(angle) + beam.y * Math.Cos(angle);

                //Set!
                beam.x = rotatedxbeam;
                beam.y = rotatedybeam;
            }        
            
            #endregion

            #region Isolate Origin Beam in RAM and Revit
            //Get that location for Origin Beam in Revit

            //Convert string to int
            int OriginElementIDint = 0;
            Int32.TryParse(ElementIDUser, out OriginElementIDint);

            ElementId OriginElementId = new ElementId(OriginElementIDint);
            Element OriginRevitBeam = doc.GetElement(OriginElementId);
            LocationCurve OriginBeamElementLocationCurve = OriginRevitBeam.Location as LocationCurve;
            Curve OriginBeamCurve = OriginBeamElementLocationCurve.Curve;

            //Normalized parameter at 0.5 for midpoint -- ***** MAKE SURE WE DONT FORGET THIS ONLY WORKS FOR STRAIGHT LINES!
            XYZ OriginBeamMidpoint = OriginBeamCurve.Evaluate(0.5, true);

            //Get Coordinates for Origin Revit Beam
            double OriginBeamRevitCPx = OriginBeamMidpoint.X;
            double OriginBeamRevitCPy = OriginBeamMidpoint.Y;
            double OriginBeamRevitCPz = OriginBeamMidpoint.Z;
            bool found = false;

            double OriginBeamSAPCPx = 0;
            double OriginBeamSAPCPy = 0;
            double OriginBeamSAPCPz = 0;

            //Get that location for Origin Beam in RAM
            foreach (BeamDataModel beam in SAPBeams)
            {
                if (beam.ID == SAPIDUser)
                {
                    OriginBeamSAPCPx = beam.x;
                    OriginBeamSAPCPy = beam.y;
                    OriginBeamSAPCPz = beam.z;
                    found = true;
                    break;
                }              
            }

            //Display warning message if not found
            if (!found)
            {
                Message.Display("SAP Origin Beam not found", WindowType.Warning);
                return;
            }

            #endregion

            #region Translate SAP Geometry

            //Translate geometry
            double deltaX = OriginBeamRevitCPx - OriginBeamSAPCPx;
            double deltaY = OriginBeamRevitCPy - OriginBeamSAPCPy;
            double deltaZ = OriginBeamRevitCPz - OriginBeamSAPCPz;

            //Translate the Beams in SAP to match the Revit Beams using the origin beam and rotation specified
            foreach (BeamDataModel beam in SAPBeams)
            {
                //Translate first
                double translatedxbeam = beam.x + deltaX;
                double translatedybeam = beam.y + deltaY;
                double translatedzbeam = beam.z + deltaZ;

                //Set!
                beam.x = translatedxbeam;
                beam.y = translatedybeam;
                beam.z = translatedzbeam;
            }

            #endregion

            #region clear Parameters if User Specified

            //If the we want to clear
            if (ClearToggleUser == true)
            {
                using (Transaction t = new Transaction(doc, "parameter"))
                {
                    t.Start("param");

                    //Clear all Beams
                    foreach (BeamDataModel RevitMember in RevitMembers)
                    {
                        foreach (string paramname in SFParamaterNames)
                        {
                            Parameter param = RevitMember.element.LookupParameter(paramname);
                            param.Set("");
                        }
                    }

                    t.Commit();
                }
            }

            #endregion

            #region Matching Scheme
            //Tolerance for matching
            double Tolerancedouble = Convert.ToDouble(ToleranceUser);

            //Begin Transaction
            using (Transaction t = new Transaction(doc, "Matching"))
            {
                t.Start("Match");
                //BEAMS
                //Start with RAM.. typically less beams in RAM Model
                foreach (BeamDataModel SAPbeam in SAPBeams)
                {
                    //Matching Beams Lists
                    List<double> MDBeams = new List<double>();
                    List<int> MDBeamsI = new List<int>();

                    //Check all Revit Beams
                    foreach (BeamDataModel RevitMember in RevitMembers)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((SAPbeam.x - RevitMember.x), 2) + Math.Pow((SAPbeam.y - RevitMember.y), 2) + Math.Pow((SAPbeam.z - RevitMember.z), 2), 0.5);

                        //If the distance is less than the tolerance, store
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDBeams.Add(Distance);
                            MDBeamsI.Add(RevitMembers.IndexOf(RevitMember));
                        }
                    }

                    //If any of the beams matched
                    if (MDBeams.Any())
                    {
                        //Matching Information
                        double MinDBM = MDBeams.Min();
                        int MinDBMI = MDBeams.IndexOf(MinDBM);
                        int MatchI = MDBeamsI[MinDBMI];
                        MDBeams.Clear();
                        MDBeamsI.Clear();

                        //FLAG TIME BABY
                        BeamDataModel MatchedRevitBeam = RevitMembers[MatchI];

                        //Set Size
                        Parameter param = MatchedRevitBeam.element.LookupParameter(SFParamaterNames[0]);
                        param.Set(SAPbeam.name);

                        //Size Flag
                        if (MatchedRevitBeam.name.ToUpper() == SAPbeam.name.ToUpper())
                        {
                            param = MatchedRevitBeam.element.LookupParameter(SFParamaterNames[1]);
                            param.Set("True");
                        }
                        else
                        {
                            param = MatchedRevitBeam.element.LookupParameter(SFParamaterNames[1]);
                            param.Set("False");
                        }
                    }
                }

                t.Commit();
            }
            #endregion

            #region colors -- Move to an independent method
            //Define Colors
            Color green = new Color(0, 255, 0);
            Color red = new Color(255, 0, 0);
            #endregion

            #region Create New 3D View ---FIX FILTER NAMES.
            //View Name
            string ViewName = "LATERAL COLUMNS QAQC SAP";

            //Initiate 3D view check
            bool exist = false;
            FilteredElementCollector all3DViews = new FilteredElementCollector(doc).OfClass(typeof(View3D));

            //Check if the view exists in the model
            foreach (View3D view in all3DViews)
            {
                if (view.Name == ViewName)
                {
                    exist = true;
                }
            }

            //If the 3D view exists, then we don't need to perform this.
            if (exist == false)
            {
                //**********************************************************THIS NEEDS TO BE SIMPLIFIED
                //Initiate all the filter rules
                //Create the List of Beam Filters
                List<FilterRule> filterRules = new List<FilterRule>();
                List<string> FilterNames = new List<String>();
                FilterNames.Add("Flag.SizeSAP");

                //Create the List of Beam Colors
                List<Color> FilterNamesColor = new List<Color>();
                FilterNamesColor.Add(red);
                //**********************************************************THIS NEEDS TO BE SIMPLIFIED


                //Need to add if statement for if the 3d view exists
                var collector = new FilteredElementCollector(doc);

                //Collect the first or default 3D view you find
                var viewFamilyType = collector
                  .OfClass(typeof(ViewFamilyType))
                  .OfType<ViewFamilyType>()
                  .FirstOrDefault(x =>
                   x.ViewFamily == ViewFamily.ThreeDimensional);

                //This is my backup for the element filter
                List<FillPatternElement> FillPatterns = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().ToList();

                //Initiate
                ElementId SolidPatternId = null;

                //Find the first solid fill pattern in the project and use that
                foreach (FillPatternElement fillpatternelement in FillPatterns)
                {
                    FillPattern fillpattern = fillpatternelement.GetFillPattern();

                    if (fillpattern.IsSolidFill)
                    {
                        SolidPatternId = fillpatternelement.Id;
                        break;
                    }
                }

                using (Transaction t = new Transaction(doc, "3dView"))
                {
                    t.Start("3dview");

                    //Create the 3D View
                    var view3D = (viewFamilyType != null)
                    ? View3D.CreateIsometric(doc, viewFamilyType.Id)
                    : null;

                    //Rename View
                    view3D.Name = ViewName;

                    view3D.ViewTemplateId = new ElementId(-1);

                    //Initialize list
                    List<ElementId> category = new List<ElementId>();

                    Categories categories = view3D.Document.Settings.Categories;

                    //Need to filter out everything in the view except beams and columns
                    foreach (Category cat in categories)
                    {
                        string name = cat.Name;
                        if (cat.get_AllowsVisibilityControl(view3D))
                        {
                            if (name == "Structural Framing" || name == "Structural Columns")
                            {
                                cat.set_Visible(view3D, true);
                            }
                            else
                            {
                                cat.set_Visible(view3D, false);
                            }
                        }
                    }

                    #region Acceptable Filter

                    //Create Acceptable Beam Filter

                    //Create beam
                    category.Add(new ElementId(BuiltInCategory.OST_StructuralFraming));
                    category.Add(new ElementId(BuiltInCategory.OST_StructuralColumns));

                    string FilterNameOverall = "Flag.SizeAcceptableSAP";

                    Color color = green;

                    //First Element to retrieve Parameters
                    Element Beam = BeamElements.First();

                    // Create the list of filter rules
                    ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, FilterNameOverall + "_RULE", category);

                    for (int i = 0; i < (FilterNames.Count); i++)
                    {
                        string FilterName = FilterNames[i];

                        //collect GUID
                        Guid spguid = Beam.LookupParameter(FilterName).GUID;

                        //Get the shared parameter from the first element
                        SharedParameterElement param = SharedParameterElement.Lookup(doc, spguid);

                        //Get Element Id
                        ElementId sharedParamId = param.Id;

                        //Create Parameter Rule
                        filterRules.Add(ParameterFilterRuleFactory.CreateEqualsRule(sharedParamId, "True", false));

                        //Cumulate all the filter rules
                        parameterFilterElement.SetElementFilter(new ElementParameterFilter(filterRules));
                    }

                    //Add those filter rules to the view
                    view3D.AddFilter(parameterFilterElement.Id);

                    //Initialize filterSettings
                    OverrideGraphicSettings filterSettings = new OverrideGraphicSettings();

                    //Set the color of the background pattern
                    filterSettings.SetSurfaceBackgroundPatternColor(color);

                    //Set Fill Pattern
                    filterSettings.SetSurfaceBackgroundPatternId(SolidPatternId);

                    //Set the filters to the new 3D view
                    view3D.SetFilterOverrides(parameterFilterElement.Id, filterSettings);

                    //Make filter visibility true
                    view3D.SetFilterVisibility(parameterFilterElement.Id, true);

                    //Clear the filter rules after its been set to ensure no carry over
                    filterRules.Clear();

                    #endregion

                    #region Filters Flags
                    //All the beam filters
                    for (int i = 0; i < (FilterNames.Count); i++)
                    {
                        string FilterName = FilterNames[i];

                        color = FilterNamesColor[i];

                        //First Element to retrieve Parameters
                        Beam = BeamElements.First();

                        // Create the list of filter rules
                        parameterFilterElement = ParameterFilterElement.Create(doc, FilterName + "_RULE", category);

                        //collect GUID
                        Guid spguid = Beam.LookupParameter(FilterName).GUID;

                        //Get the shared parameter from the first element
                        SharedParameterElement param = SharedParameterElement.Lookup(doc, spguid);

                        //Get Element Id
                        ElementId sharedParamId = param.Id;

                        //Create Parameter Rule
                        filterRules.Add(ParameterFilterRuleFactory.CreateEqualsRule(sharedParamId, "False", false));

                        //Cumulate all the filter rules
                        parameterFilterElement.SetElementFilter(new ElementParameterFilter(filterRules));

                        //Add those filter rules to the view
                        view3D.AddFilter(parameterFilterElement.Id);

                        //Initialize filterSettings
                        filterSettings = new OverrideGraphicSettings();

                        //Set the color of the background pattern
                        filterSettings.SetSurfaceBackgroundPatternColor(color);

                        //Set Fill Pattern
                        filterSettings.SetSurfaceBackgroundPatternId(SolidPatternId);

                        //Set the filters to the new 3D view
                        view3D.SetFilterOverrides(parameterFilterElement.Id, filterSettings);

                        //Make filter visibility true
                        view3D.SetFilterVisibility(parameterFilterElement.Id, true);

                        //Clear the filter rules after its been set to ensure no carry over
                        filterRules.Clear();
                    }
                    #endregion

                    
                    t.Commit();
                }
            }

            //Set 3D view as active view
            foreach (View3D view in all3DViews)
            {
                if (view.Name == ViewName)
                {
                    uidoc.ActiveView = view;
                    break;
                }
            }
            #endregion

            //Close the form
            qaqcform.Close();

            //Display message to user that they are a successful human
            Message.Display("Models successfully compared", WindowType.Information);
        }

        /// <summary>
        /// Name Method
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "QAQC Run External Event";
        }
        

    }
}
