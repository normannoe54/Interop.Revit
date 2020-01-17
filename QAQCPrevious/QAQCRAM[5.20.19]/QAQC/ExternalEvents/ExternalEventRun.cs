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
using RAMDATAACCESSLib;
#endregion

namespace QAQCRAM
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventRun : IExternalEventHandler
    {
        #region Internal RAM Functions
        //Variables imported and exported during the process
        internal SCoordinate StartPointC { get; set; }
        internal SCoordinate EndPointC { get; set; }
        internal SCoordinate StartPointB { get; set; }
        internal SCoordinate EndPointB { get; set; }
        internal int Size { get; set; }
        internal object NumberofStuds { get; set; }
        #endregion

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
            string sharedParameterFile = directoryName + "\\SharedParam.txt";

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

            //Collect Structural Column Parameters
            List<string> ColParamaterNames = InternalConstants.StructuralColumnParameters();

            //Collection Vertical Braces Parameters
            List<string> VBParamaterNames = InternalConstants.StructuralVBParameters();

            //List of parameter names to create
            List<string> CreateParamaterNames = new List<string>();

            //Create complete list
            CreateParamaterNames = SFParamaterNames.Concat(ColParamaterNames).Concat(VBParamaterNames).ToList();

            #endregion

            #region Label category sets (Structural Framing, Columns, VB)
            Category SFcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
            CategorySet SFcatset = app.Create.NewCategorySet();
            SFcatset.Insert(SFcat);

            Category Colcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
            CategorySet Colcatset = app.Create.NewCategorySet();
            Colcatset.Insert(Colcat);

            Category VBcat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
            CategorySet VBcatset = app.Create.NewCategorySet();
            VBcatset.Insert(VBcat);
            #endregion

            #region Create Shared Parameters
            Parameters.CreateSharedParameters(SFParamaterNames, SFcatset, doc, app, QAQCgroup);
            Parameters.CreateSharedParameters(ColParamaterNames, Colcatset, doc, app, QAQCgroup);
            Parameters.CreateSharedParameters(VBParamaterNames, VBcatset, doc, app, QAQCgroup);
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
                    studs = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS).AsString()),
                    camber = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER).AsString())
                };

                RevitBeams.Add(Beam);
            }
              
            #endregion

            #region Collect Column Elements [Revit]

            //Retrieve Columns
            IList<Element> ColumnElements = RevitCollectElement.ColElements(doc);

            //Initialize Beam Model
            List<ColumnDataModel> RevitColumns = new List<ColumnDataModel>();

            foreach (Element ColumnElement in ColumnElements)
            {
                var Column = new ColumnDataModel
                {
                    x = CollectLocation.StructuralColumnMidpoint(ColumnElement).X,
                    y = CollectLocation.StructuralColumnMidpoint(ColumnElement).Y,
                    z = CollectLocation.StructuralColumnMidpoint(ColumnElement).Z,
                    name = ColumnElement.Name.ToUpper(),
                    rotation = ColumnData.ColumnRotation(ColumnElement)
                };

                RevitColumns.Add(Column);
            }
               
            #endregion

            #region Collect Structural Braces Elements [Revit]

            //Retrieve VB
            IList<Element> VBElements = RevitCollectElement.VBElements(doc);

            //Initialize VB Model
            List<VBDataModel> RevitVBs = new List<VBDataModel>();

            foreach (Element VBElement in VBElements)
            {
                var VB = new VBDataModel
                {
                    x = CollectLocation.StructuralFramingMidpoint(VBElement).X,
                    y = CollectLocation.StructuralFramingMidpoint(VBElement).Y,
                    z = CollectLocation.StructuralFramingMidpoint(VBElement).Z,
                    name = VBElement.Name.ToUpper(),

                };

                RevitVBs.Add(VB);
            }

            #endregion

            #region Collect Inputs from Userform
            //Retrieve inputs from Userform
            string FilenameUser = qaqcform.FilenameText.Text;
            string ElementIDUser = qaqcform.ElementIDText.Text;
            string RAMIDUser = qaqcform.RAMIDText.Text;
            string RAMStoryUser = qaqcform.RAMStoryText.Text;
            string RotationUser = qaqcform.RotText.Text;
            string ToleranceUser = qaqcform.ToleranceText.Text;
            bool ClearToggleUser = qaqcform.ClearToggle.Checked;
            #endregion

            #region Catch for open RAM Model
            string USRCheck = FilenameUser.Replace(@".rss", ".usr");
            bool CheckExists = File.Exists(USRCheck);

            if (CheckExists == true)
            {
                Message.Display("Please close the RAM Model you specified", WindowType.Warning);
                return;
            }
            #endregion

            #region Collect RAM Beams and Columns
            //Initialize method
            CollectInfo RAMCollection = new CollectInfo();

            //Create instance of RAM DATA ACCESS
            RamDataAccess1 RAM = new RamDataAccess1();

            //Initiate IDBIO1 Interface
            IDBIO1 RAMIDBIO1 = RAM.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);

            //Load Model Data from a file name (OpenFile)
            double LOADDB = RAMIDBIO1.LoadDataBase2(FilenameUser, "DA");

            //Load the model data
            IModel Imodel = RAMIDBIO1.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);

            //Get the beams
            List<BeamDataModel>RAMStlBeams = RAMCollection.GetBeams(Imodel);

            //Get the joists
            List<BeamDataModel>RAMJoists = RAMCollection.GetJoists(Imodel);

            //Concat Stl beams and Joists
            List<BeamDataModel> RAMBeams = RAMStlBeams.Concat(RAMJoists).ToList();

            //Get the Columns
            List <ColumnDataModel> RAMColumns = RAMCollection.GetColumns(Imodel);

            //Get the Columns
            List<VBDataModel> RAMVBs = RAMCollection.GetVB(Imodel);

            //Close the database
            RAMIDBIO1.CloseDatabase();
            #endregion

            #region Rotate the RAM Beam and Columns

            //Rotation
            //Convert string to int
            double Rotationdouble = Convert.ToDouble(RotationUser);

            //Convert Angle to radians
            double angle = Rotationdouble*(Math.PI/180);

            //Translate the Beams in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (BeamDataModel beam in RAMBeams)
            {
                //Rotate the beam geometry
                double rotatedxbeam = beam.x * Math.Cos(angle) - beam.y * Math.Sin(angle);
                double rotatedybeam = beam.x * Math.Sin(angle) + beam.y * Math.Cos(angle);

                //Set!
                beam.x = rotatedxbeam;
                beam.y = rotatedybeam;
            }

            //Translate the Columns in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (ColumnDataModel column in RAMColumns)
            {
                //Rotate the beam geometry
                double rotatedxcolumn = column.x * Math.Cos(angle) - column.y * Math.Sin(angle);
                double rotatedycolumn = column.x * Math.Sin(angle) + column.y * Math.Cos(angle);

                //Set!
                column.x = rotatedxcolumn;
                column.y = rotatedycolumn;
                column.rotation = SimpleRefine.SymetricalRotation(Math.Round(column.rotation + Rotationdouble)); 
            }

            //Translate the VB in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (VBDataModel vb in RAMVBs)
            {
                //Rotate the beam geometry
                double rotatedxVB = vb.x * Math.Cos(angle) - vb.y * Math.Sin(angle);
                double rotatedyVB = vb.x * Math.Sin(angle) + vb.y * Math.Cos(angle);

                //Set!
                vb.x = rotatedxVB;
                vb.y = rotatedyVB;
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

            double OriginBeamRAMCPx = 0;
            double OriginBeamRAMCPy = 0;
            double OriginBeamRAMCPz = 0;

            //Get that location for Origin Beam in RAM
            foreach (BeamDataModel beam in RAMBeams)
            {
                if (beam.story == RAMStoryUser)
                {
                    if (beam.ID == RAMIDUser)
                    {
                        OriginBeamRAMCPx = beam.x;
                        OriginBeamRAMCPy = beam.y;
                        OriginBeamRAMCPz = beam.z;
                        found = true;
                        break;
                    }
                }
            }

            //Display warning message if not found
            if (!found)
            {
                Message.Display("RAM Origin Beam not found", WindowType.Warning);
                return;
            }

            #endregion

            #region Translate Beam, Column, and VB Geometry

            //Translate geometry
            double deltaX = OriginBeamRevitCPx- OriginBeamRAMCPx;
            double deltaY = OriginBeamRevitCPy - OriginBeamRAMCPy;
            double deltaZ = OriginBeamRevitCPz - OriginBeamRAMCPz;

            //Translate the Beams in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (BeamDataModel beam in RAMBeams)
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

            //Translate the Columns in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (ColumnDataModel column in RAMColumns)
            {
                //Translate first
                double translatedxcolumn = column.x + deltaX;
                double translatedycolumn = column.y + deltaY;
                double translatedzcolumn = column.z + deltaZ;

                //Set!
                column.x = translatedxcolumn;
                column.y = translatedycolumn;
                column.z = translatedzcolumn;
            }

            //Translate the VBs in RAM to match the Revit Beams using the origin beam and rotation specified
            foreach (VBDataModel vb in RAMVBs)
            {
                //Translate first
                double translatedxvb = vb.x + deltaX;
                double translatedyvb = vb.y + deltaY;
                double translatedzvb = vb.z + deltaZ;

                //Set!
                vb.x = translatedxvb;
                vb.y = translatedyvb;
                vb.z = translatedzvb;
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
                    foreach (Element BeamElement in BeamElements)
                    {
                        foreach (string paramname in SFParamaterNames)
                        {
                            Parameter param = BeamElement.LookupParameter(paramname);                     
                            param.Set("");                                 
                        }
                    }

                    //Clear all Columns
                    foreach (Element ColumnElement in ColumnElements)
                    {
                        foreach (string paramname in ColParamaterNames)
                        {
                            Parameter param = ColumnElement.LookupParameter(paramname);
                            param.Set("");
                        }
                    }

                    ////Clear all VB
                    foreach (Element VBElement in VBElements)
                    {
                        foreach (string paramname in VBParamaterNames)
                        {
                            Parameter param = VBElement.LookupParameter(paramname);
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
                foreach (BeamDataModel RAMbeam in RAMBeams)
                {
                    //Matching Beams Lists
                    List<double> MDBeams = new List<double>();
                    List<int> MDBeamsI = new List<int>();

                    //Check all Revit Beams
                    foreach (BeamDataModel Revitbeam in RevitBeams)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((RAMbeam.x - Revitbeam.x), 2) + Math.Pow((RAMbeam.y - Revitbeam.y), 2) + Math.Pow((RAMbeam.z - Revitbeam.z), 2), 0.5);

                        //If the distance is less than the tolerance, store 
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDBeams.Add(Distance);
                            MDBeamsI.Add(RevitBeams.IndexOf(Revitbeam));
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

                        //Set Camber
                        Parameter param = BeamElements[MatchI].LookupParameter(SFParamaterNames[0]);
                        param.Set(RAMbeam.camber);

                        //Set Studs
                        param = BeamElements[MatchI].LookupParameter(SFParamaterNames[1]);
                        param.Set(RAMbeam.studs);

                        //Set Size
                        param = BeamElements[MatchI].LookupParameter(SFParamaterNames[2]);
                        param.Set(RAMbeam.name);

                        //FLAG TIME BABY
                        //Camber Flag
                        //There needs to be a parser for camber to equal Rams output\
                        BeamDataModel MatchedRevitBeam = RevitBeams[MatchI];

                        if (MatchedRevitBeam.camber == RAMbeam.camber)
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[3]);
                            param.Set("True");
                        }
                        else
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[3]);
                            param.Set("False");
                        }

                        //Studs Flag
                        if (MatchedRevitBeam.studs.ToUpper() == RAMbeam.studs.ToUpper())
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[4]);
                            param.Set("True");
                        }
                        else
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[4]);
                            param.Set("False");
                        }
                        //Size Flag
                        if (MatchedRevitBeam.name.ToUpper() == RAMbeam.name.ToUpper())
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[5]);
                            param.Set("True");
                        }
                        else
                        {
                            param = BeamElements[MatchI].LookupParameter(SFParamaterNames[5]);
                            param.Set("False");
                        }
                    }
                }


                //Column Matching Scheme
                foreach (ColumnDataModel RAMcolumn in RAMColumns)
                {
                    //Matching Beams Lists
                    List<double> MDColumns = new List<double>();
                    List<int> MDColumnsI = new List<int>();

                    //Check all Revit Beams
                    foreach (ColumnDataModel Revitcolumn in RevitColumns)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((RAMcolumn.x - Revitcolumn.x), 2) + Math.Pow((RAMcolumn.y - Revitcolumn.y), 2) + Math.Pow((RAMcolumn.z - Revitcolumn.z), 2), 0.5);

                        //If the distance is less than the tolerance, store 
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDColumns.Add(Distance);
                            MDColumnsI.Add(RevitColumns.IndexOf(Revitcolumn));
                        }
                    }

                    //If there were any matches
                    if (MDColumns.Any())
                    {
                        //Matching Information
                        double MinDCol = MDColumns.Min();
                        int MinDColI = MDColumns.IndexOf(MinDCol);
                        int MatchI = MDColumnsI[MinDColI];
                        MDColumns.Clear();
                        MDColumnsI.Clear();

                        //Set Size
                        Parameter param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[0]);
                        param.Set(RAMcolumn.name);

                        //Set Orient
                        param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[1]);
                        param.Set(RAMcolumn.rotation.ToString());

                        //There needs to be a parser for camber to equal Rams output
                        ColumnDataModel MatchedRevitColumn = RevitColumns[MatchI];

                        //Size Flag
                        if (MatchedRevitColumn.name.ToUpper() == RAMcolumn.name.ToUpper())
                        {
                            param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[2]);
                            param.Set("True");
                        }
                        else
                        {
                            param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[2]);
                            param.Set("False");
                        }

                        //Orient Flag
                        if (Math.Abs(MatchedRevitColumn.rotation - RAMcolumn.rotation) == 0 || Math.Abs(MatchedRevitColumn.rotation - RAMcolumn.rotation) == 180)
                        {
                            param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[3]);
                            param.Set("True");
                        }
                        else
                        {
                            param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[3]);
                            param.Set("False");
                        }
                    }                                     
                }

                //VB Matching Scheme
                foreach (VBDataModel RAMvb in RAMVBs)
                {
                    //Matching Beams Lists
                    List<double> MDVBs = new List<double>();
                    List<int> MDVBsI = new List<int>();

                    //Check all Revit Beams
                    foreach (VBDataModel Revitvb in RevitVBs)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((RAMvb.x - Revitvb.x), 2) + Math.Pow((RAMvb.y - Revitvb.y), 2) + Math.Pow((RAMvb.z - Revitvb.z), 2), 0.5);

                        //If the distance is less than the tolerance, store 
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDVBs.Add(Distance);
                            MDVBsI.Add(RevitVBs.IndexOf(Revitvb));
                        }
                    }

                    //If there were any matches
                    if (MDVBs.Any())
                    {
                        //Matching Information
                        double MinDVB = MDVBs.Min();
                        int MinDVBI = MDVBs.IndexOf(MinDVB);
                        int MatchI = MDVBsI[MinDVBI];
                        MDVBs.Clear();
                        MDVBsI.Clear();

                        //Set Size
                        Parameter param = VBElements[MatchI].LookupParameter(VBParamaterNames[0]);
                        param.Set(RAMvb.name);

                        //There needs to be a parser for camber to equal Rams output
                        VBDataModel MatchedRevitVB = RevitVBs[MatchI];

                        //Size Flag
                        if (MatchedRevitVB.name.ToUpper() == RAMvb.name.ToUpper())
                        {
                            param = VBElements[MatchI].LookupParameter(VBParamaterNames[1]);
                            param.Set("True");
                        }
                        else
                        {
                            param = VBElements[MatchI].LookupParameter(VBParamaterNames[1]);
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
            Color blue = new Color(0, 0, 255);
            Color orange = new Color(255, 128, 0);
            Color purple = new Color(255, 0, 255);
            #endregion

            #region Create New 3D View
            //View Name
            string ViewName = "Norman Has Control Now";

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
                List<string> FilterNamesBeams = new List<String>();
                FilterNamesBeams.Add("Flag.BeamSize");
                FilterNamesBeams.Add("Flag.Studs");
                FilterNamesBeams.Add("Flag.Camber");

                //Create the List of Beam Colors
                List<Color> FilterNamesBeamsColor = new List<Color>();
                FilterNamesBeamsColor.Add(red);
                FilterNamesBeamsColor.Add(blue);
                FilterNamesBeamsColor.Add(purple);

                //List of colors for columns
                List<string> FilterNamesColumns = new List<String>();
                FilterNamesColumns.Add("Flag.ColumnSize");
                FilterNamesColumns.Add("Flag.ColumnOrient");

                //Colors for Columns
                List<Color> FilterNamesColumnsColor = new List<Color>();
                FilterNamesColumnsColor.Add(red);
                FilterNamesColumnsColor.Add(orange);

                //List of colors for VB
                List<string> FilterNamesVBs = new List<String>();
                FilterNamesVBs.Add("Flag.VBSize");

                //Colors for Columns
                List<Color> FilterNamesVBsColor = new List<Color>();
                FilterNamesVBsColor.Add(red);
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

                    #region Acceptable Beam Filter

                    //Create Acceptable Beam Filter

                    //Create beam
                    category.Add(new ElementId(BuiltInCategory.OST_StructuralFraming));

                    string FilterNameOverall = "FlagBeam.SizeAcceptable";

                    Color color = green;

                    //First Element to retrieve Parameters
                    Element Beam = BeamElements.First();

                    // Create the list of filter rules
                    ParameterFilterElement parameterFilterElement = ParameterFilterElement.Create(doc, FilterNameOverall + "_RULE", category);

                    for (int i = 0; i < (FilterNamesBeams.Count); i++)
                    {
                        string FilterName = FilterNamesBeams[i];

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

                    #region Beam Filters Flags
                    //All the beam filters
                    for (int i = 0; i < (FilterNamesBeams.Count); i++)
                    {
                        string FilterName = FilterNamesBeams[i];

                        color = FilterNamesBeamsColor[i];
                    
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

                    //Change Category to Column
                    #region Accetpable Column Filter
                    category.Clear();
                    category.Add(new ElementId(BuiltInCategory.OST_StructuralColumns));

                    FilterNameOverall = "FlagColumn.SizeAcceptable";

                    color = green;

                    //First Element to retrieve Parameters
                    Element Column = ColumnElements.First();

                    // Create the list of filter rules
                    parameterFilterElement = ParameterFilterElement.Create(doc, FilterNameOverall + "_RULE", category);

                    for (int i = 0; i < (FilterNamesColumns.Count); i++)
                    {
                        string FilterName = FilterNamesColumns[i];

                        //collect GUID
                        Guid spguid = Column.LookupParameter(FilterName).GUID;

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

                    #endregion

                    #region Column Filters Flags
                    //All the column filters
                    for (int i = 0; i < (FilterNamesColumns.Count); i++)
                    {
                        string FilterName = FilterNamesColumns[i];

                        color = FilterNamesColumnsColor[i];

                        //First Element to retrieve Parameters
                        Column = ColumnElements.First();

                        // Create the list of filter rules
                        parameterFilterElement = ParameterFilterElement.Create(doc, FilterName + "_RULE", category);

                        //collect GUID
                        Guid spguid = Column.LookupParameter(FilterName).GUID;

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

                    #region Acceptable VB Filter

                    //Create Acceptable VB Filter

                    //Create beam
                    category.Clear();
                    category.Add(new ElementId(BuiltInCategory.OST_StructuralFraming));

                    FilterNameOverall = "FlagVB.SizeAcceptable";

                    color = green;

                    //First Element to retrieve Parameters
                    Element VB = VBElements.First();

                    // Create the list of filter rules
                    parameterFilterElement = ParameterFilterElement.Create(doc, FilterNameOverall + "_RULE", category);

                    for (int i = 0; i < (FilterNamesVBs.Count); i++)
                    {
                        string FilterName = FilterNamesVBs[i];

                        //collect GUID
                        Guid spguid = VB.LookupParameter(FilterName).GUID;

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

                    #endregion

                    #region VB Filters Flags
                    //All the beam filters
                    for (int i = 0; i < (FilterNamesVBs.Count); i++)
                    {
                        string FilterName = FilterNamesVBs[i];

                        color = FilterNamesVBsColor[i];

                        //First Element to retrieve Parameters
                        Beam = VBElements.First();

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
