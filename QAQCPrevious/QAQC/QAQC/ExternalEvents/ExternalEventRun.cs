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

namespace QAQC
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

            //Initialize List -------------DELETE
            List<string> BeamCamberRevit = new List<string>();
            List<string> BeamStudsRevit = new List<string>();
            List<string> BeamNameRevit = new List<string>();
            List<double> BCPxRevit = new List<double>();
            List<double> BCPyRevit = new List<double>();
            List<double> BCPzRevit = new List<double>();

            //Iterate Beam Elements and set to data model
            foreach (Element BeamElement in BeamElements)
            {
                var Beam = new BeamDataModel
                {
                    x = CollectLocation.StructuralFramingMidpoint(BeamElement).X,
                    y = CollectLocation.StructuralFramingMidpoint(BeamElement).Y,
                    z = CollectLocation.StructuralFramingMidpoint(BeamElement).Z,
                    name = BeamElement.Name.ToUpper(),
                    studs = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER).AsString()),
                    camber = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS).AsString())
                };

                RevitBeams.Add(Beam);
                //}

                //Get those cambers and studs
                string BeamCamber = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER).AsString());
                string BeamStuds = SimpleRefine.StringTrimmer(BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS).AsString());

                
                //Remove nulls and empties for camber
                if (BeamCamber == null || BeamCamber == "")
                {
                    BeamCamber = "0";
                }
                else
                {
                    if (BeamCamber.Contains("\""))
                    {
                        BeamCamber = BeamCamber.Trim().Replace("\"", "");
                    }
                }

                //Remove nulls and empties for studs
                if (BeamStuds == null || BeamStuds == "")
                {
                    BeamStuds = "0";
                }

                //Write it to a usable list
                BeamCamberRevit.Add(BeamCamber);
                BeamStudsRevit.Add(BeamStuds);
                BeamNameRevit.Add(BeamElement.Name.ToUpper());

                //Get that location for Beam
                LocationCurve BeamElementLocationCurve = BeamElement.Location as LocationCurve;
                Curve BeamCurve = BeamElementLocationCurve.Curve;

                //Normalized parameter at 0.5 for midpoint -- ***** MAKE SURE WE DONT FORGET THIS ONLY WORKS FOR STRAIGHT LINES!
                XYZ BeamMidpoint = BeamCurve.Evaluate(0.5, true);

                //Get Coordinates
                BCPxRevit.Add(BeamMidpoint.X);
                BCPyRevit.Add(BeamMidpoint.Y);
                BCPzRevit.Add(BeamMidpoint.Z);
            }

            #endregion

            #region Collect Column Elements [Revit]

            //Retrieve Columns
            IList<Element> ColumnElements = RevitCollectElement.ColElements(doc);

            //Initialize Beam Model
            List<ColumnDataModel> RevitColumns = new List<ColumnDataModel>();

            //Initialize List ----- DELETE 
            List<string> ColumnNameRevit = new List<string>();
            List<double> ColumnRotationRevit = new List<double>();
            List<double> CCPxRevit = new List<double>();
            List<double> CCPyRevit = new List<double>();
            List<double> CCPzRevit = new List<double>();

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
                //}

                //constructors
                double Revitvalue = 0;

                //Get that location for column
                LocationPoint ColumnElementLocationPoint = ColumnElement.Location as LocationPoint;

                //if null then its slanted column or is not a 
                if (ColumnElementLocationPoint == null)
                {
                    //Get that location for column
                    LocationCurve ColumnElementLocationCurve = ColumnElement.Location as LocationCurve;

                    //Make sure there is no null locations
                    if (ColumnElementLocationCurve != null)
                    {
                        //Get the Name
                        ColumnNameRevit.Add(ColumnElement.Name.ToUpper());

                        Curve ColumnCurve = ColumnElementLocationCurve.Curve;

                        //Normalized parameter at 0.5 for midpoint -- ***** MAKE SURE WE DONT FORGET THIS ONLY WORKS FOR STRAIGHT LINES!
                        XYZ ColumnMidpoint = ColumnCurve.Evaluate(0.5, true);
                        CCPxRevit.Add(ColumnMidpoint.X);
                        CCPyRevit.Add(ColumnMidpoint.Y);
                        CCPzRevit.Add(ColumnMidpoint.Z);

                        //collect "Cross-Section Rotation" Parameter if slanted
                        BuiltInParameter Revitvalueparam = BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE;
                        double rotationdouble = ColumnElement.get_Parameter(Revitvalueparam).AsDouble() * (180 / Math.PI);
                        Revitvalue = Math.Round(rotationdouble);

                        //Parser
                        double RotationAdjusted = SimpleRefine.SymetricalRotation(Revitvalue);

                        //Adjust 90 degree difference in RAM values
                        ColumnRotationRevit.Add(RotationAdjusted);
                    }
                }
                //Else then its a vertical column
                else
                {
                    //Get the Name
                    ColumnNameRevit.Add(ColumnElement.Name.ToUpper());

                    //Basepoint for X and Y coords
                    XYZ ColumnBasePoint = ColumnElementLocationPoint.Point;
                    CCPxRevit.Add(ColumnBasePoint.X);
                    CCPyRevit.Add(ColumnBasePoint.Y);

                    //Bounding box for Z coords
                    BoundingBoxXYZ bb = ColumnElement.get_BoundingBox(null);

                    if (null == bb)
                    {
                        throw new ArgumentException(
                          "Expected Element 'Column' to have a valid bounding box.");
                    }

                    CCPzRevit.Add(((bb.Max.Z - bb.Min.Z) * 0.5) + bb.Min.Z);

                    //collect "Cross-Section Rotation" Parameter if slanted
                    double rotationdouble = ColumnElementLocationPoint.Rotation * (180 / Math.PI);
                    Revitvalue = Math.Round(rotationdouble);

                    //Parser
                    double RotationAdjusted = SimpleRefine.SymetricalRotation(Revitvalue);

                    ColumnRotationRevit.Add(RotationAdjusted);
                }
            }

            #endregion

            #region Collect Structural Braces Elements [Revit]

            //Retrieve VB
            IList<Element> VBElements = RevitCollectElement.VBElements(doc);

            //Initialize VB Model
            List<VBDataModel> RevitVB = new List<VBDataModel>();

            foreach (Element VBElement in VBElements)
            {
                var VB = new VBDataModel
                {
                    x = CollectLocation.StructuralColumnMidpoint(VBElement).X,
                    y = CollectLocation.StructuralColumnMidpoint(VBElement).Y,
                    z = CollectLocation.StructuralColumnMidpoint(VBElement).Z,
                    name = VBElement.Name.ToUpper(),
                };

                RevitVB.Add(VB);
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

           
            #region Remove
            //Initialize List--CAN DELETE
            List<string> BeamCamberRAM = new List<string>();
            List<string> BeamStudsRAM = new List<string>();
            List<string> BeamNameRAM = new List<string>();
            List<int> BeamIDRAM = new List<int>();
            List<string> BeamStoryRAM = new List<string>();
            List<double> BeamxSRAM = new List<double>();
            List<double> BeamySRAM = new List<double>();
            List<double> BeamzSRAM = new List<double>();
            List<double> BeamxERAM = new List<double>();
            List<double> BeamyERAM = new List<double>();
            List<double> BeamzERAM = new List<double>();

            //Initialize List--CAN DELETE
            List<string> ColumnNameRAM = new List<string>();
            List<double> ColumnRotationRAM = new List<double>();
            List<int> ColumnSpliceRAM = new List<int>();
            List<int> ColumnIDRAM = new List<int>();
            List<double> ColumnxSRAM = new List<double>();
            List<double> ColumnySRAM = new List<double>();
            List<double> ColumnzSRAM = new List<double>();
            List<double> ColumnxERAM = new List<double>();
            List<double> ColumnyERAM = new List<double>();
            List<double> ColumnzERAM = new List<double>();
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
            List<BeamDataModel>RAMBeams = RAMCollection.GetBeams(Imodel);

            //Get the Columns
            List<ColumnDataModel> RAMColumns = RAMCollection.GetColumns(Imodel);
















            //Get Story Data
            IStories Istories = Imodel.GetStories();

            //Get Number of Stories
            int NumStories = Istories.GetCount();

            //Collect Columns from all stories
            for (int Story = 0; Story < (NumStories); Story++)
            {
                //Get Story Object Definition at specified Story
                IStory Istory = Istories.GetAt(Story);

                //Determine Story ID
                string strStoryID = Istory.strLabel;

                //Get all the columns at the specified story
                IColumns Icolumns = Istory.GetColumns();

                //Filer Steel Columns  
                Icolumns.Filter(EColumnFilter.eColFilter_Material, EMATERIALTYPES.EAnyMaterial);

                //Determine number of columns
                int NumColumns = Icolumns.GetCount();

                //Get all the beams at the specified story
                IBeams Ibeams = Istory.GetBeams();

                //Filer Steel Beams 
                Ibeams.Filter(EBeamFilter.eBeamFilter_Material, EMATERIALTYPES.EAnyMaterial);

                //Determine number of beams
                int NumBeams = Ibeams.GetCount();

                //Collect information on the multiple columns at that specific level
                for (int Column = 0; Column < (NumColumns); Column++)
                {
                    //Get each individual columns
                    IColumn Icolumn = Icolumns.GetAt(Column);

                    //Get Column ID Number
                    //int IMemberID = Icolumn.lUID;
                    //ColumnIDRAM.Add(IMemberID);

                    //Get Member Number
                    int MemberNumber = Icolumn.lLabel;
                    ColumnIDRAM.Add(MemberNumber);

                    //Get Section Property
                    ColumnNameRAM.Add(Icolumn.strSectionLabel);

                    //Get Start and End Coordinates
                    SCoordinate StartPointC = new SCoordinate();
                    SCoordinate EndPointC = new SCoordinate();
                    Icolumn.GetEndCoordinates(ref StartPointC, ref EndPointC);

                    double x1C = EndPointC.dXLoc / 12;
                    double y1C = EndPointC.dYLoc / 12;
                    double z1C = EndPointC.dZLoc / 12;

                    double x2C = StartPointC.dXLoc / 12;
                    double y2C = StartPointC.dYLoc / 12;
                    double z2C = StartPointC.dZLoc / 12;

                    ColumnxSRAM.Add(x1C);
                    ColumnySRAM.Add(y1C);
                    ColumnzSRAM.Add(z1C);

                    ColumnxERAM.Add(x2C);
                    ColumnyERAM.Add(y2C);
                    ColumnzERAM.Add(z2C);

                    //Determine Orientation of the column
                    //Parser
                    double RotationAdjusted = SimpleRefine.SymetricalRotation(Math.Round(Icolumn.dOrientation));

                    //Theres a 90 degree difference in the global coordinates from RAM and Revit
                    RotationAdjusted = Math.Abs(RotationAdjusted - 90);
                    ColumnRotationRAM.Add(RotationAdjusted);

                    //Check if this is a splice Level
                    ColumnSpliceRAM.Add(Icolumn.bSpliceLevel);                  
                }


                //Collect information on the multiple columns at that specific level
                for (int Beam = 0; Beam < (NumBeams); Beam++)
                {
                    //Get each individual columns
                    IBeam Ibeam = Ibeams.GetAt(Beam);

                    //Get Member Number
                    int MemberNumberB = Ibeam.lLabel;
                    BeamIDRAM.Add(MemberNumberB);

                    //Get Section Property
                    BeamNameRAM.Add(Ibeam.strSectionLabel);

                    //Get Start and End Coordinates
                    SCoordinate StartPointB = new SCoordinate();
                    SCoordinate EndPointB = new SCoordinate();
                    EBeamCoordLoc eBeamCoordLoc = new EBeamCoordLoc();
                    Ibeam.GetCoordinates(eBeamCoordLoc, ref StartPointB, ref EndPointB);

                    double x1B = StartPointB.dXLoc / 12;
                    double y1B = StartPointB.dYLoc / 12;
                    double z1B = StartPointB.dZLoc / 12;

                    double x2B = EndPointB.dXLoc / 12;
                    double y2B = EndPointB.dYLoc / 12;
                    double z2B = EndPointB.dZLoc / 12;

                    BeamxSRAM.Add(x1B);
                    BeamySRAM.Add(y1B);
                    BeamzSRAM.Add(z1B);

                    BeamxERAM.Add(x2B);
                    BeamyERAM.Add(y2B);
                    BeamzERAM.Add(z2B);

                    //Determine camber
                    double camber = Ibeam.dCamber;
                    string camberstring = camber.ToString();

                    //Change data
                    string camberfracstring = SimpleRefine.Decimaltofractions(camberstring);
                    BeamCamberRAM.Add(camberfracstring);

                    //Determine Story
                    BeamStoryRAM.Add(strStoryID);

                    //Determine studs
                    ISteelBeamDesignResult SteelBeamDesign = Ibeam.GetSteelDesignResult();
                    DAArray numStudsDAArray = SteelBeamDesign.GetNumStudsInSegments();
                    string strNumberofStuds = "";
                    List<string> StudArray = new List<string>();

                    if (Ibeam.bComposite == 1)
                    {
                        int Size = 0;
                        object NumberofStuds = 0;

                        numStudsDAArray.GetSize(ref Size);

                        for (int studseg = 0; studseg < Size; studseg++)

                        {
                            numStudsDAArray.GetAt(studseg, ref NumberofStuds);

                            strNumberofStuds = NumberofStuds.ToString();

                            if (strNumberofStuds == "")
                            {
                                strNumberofStuds = "0";
                            }
                            StudArray.Add(strNumberofStuds);
                        }

                        BeamStudsRAM.Add(String.Join(";", StudArray.ToArray()));
                        StudArray.Clear();
                    }
                    else
                    {
                        BeamStudsRAM.Add("0");
                    }
                }
            }
            RAMIDBIO1.CloseDatabase();
            #endregion

            #region RAM Column Splice Correction

            //Constructors
            List<int> IndexSplice = new List<int>();
            double Indicator = 0;
            double StartX = 0;
            double StartY = 0;
            double StartZ = 0;
            double EndX = 0;
            double EndY = 0;
            double EndZ = 0;

            //New List Constructors
            List<string> ColumnNameRAMRevised = new List<string>();
            List<double> ColumnRotationRAMRevised = new List<double>();
            List<double> ColumnxSRAMRevised = new List<double>();
            List<double> ColumnySRAMRevised = new List<double>();
            List<double> ColumnzSRAMRevised = new List<double>();
            List<double> ColumnxERAMRevised = new List<double>();
            List<double> ColumnyERAMRevised = new List<double>();
            List<double> ColumnzERAMRevised = new List<double>();
            List<double> CCPxRAM = new List<double>();
            List<double> CCPyRAM = new List<double>();
            List<double> CCPzRAM = new List<double>();
            bool found = false;

            //Iterate through list and trim te columns that are spliced
            for (int i = 0; i < (ColumnIDRAM.Count()); i++)
            {
                //If the column has already been spliced
                if (ColumnSpliceRAM[i] == 0 && !IndexSplice.Contains(i))
                {
                    StartX = ColumnxSRAM[i];
                    StartY = ColumnySRAM[i];
                    StartZ = ColumnzSRAM[i];
                    EndX = ColumnxERAM[i];      
                    EndY = ColumnyERAM[i];
                    EndZ = ColumnzERAM[i];
                    Indicator = 0;
                    
                    while (Indicator !=1)
                    {
                        //allows for a for - else statement
                        found = false;

                        for (int j = 0; j < (ColumnIDRAM.Count()); j++)
                        {

                            //Does the endpoint match the startpoint
                            if ((Math.Round(ColumnxSRAM[j],3) == Math.Round(EndX,3)) && (Math.Round(ColumnySRAM[j], 3) == Math.Round(EndY,3)) && (Math.Round(ColumnzSRAM[j], 3) == Math.Round(EndZ,3)) && !IndexSplice.Contains(j))
                            {
                                //If the column continues up the height of the building
                                if (ColumnSpliceRAM[j] ==0)
                                {
                                    EndX = ColumnxERAM[j];
                                    EndY = ColumnyERAM[j];
                                    EndZ = ColumnzERAM[j];

                                    //Determine index that has been accounted for
                                    IndexSplice.Add(j);
                                    found = true;
                                    break;
                                }
                                //The column is spliced and collect the data
                                else
                                {
                                    EndX = ColumnxERAM[j];
                                    EndY = ColumnyERAM[j];
                                    EndZ = ColumnzERAM[j];
                                    Indicator = 1;
                                    ColumnxSRAMRevised.Add(StartX);
                                    ColumnySRAMRevised.Add(StartY);
                                    ColumnzSRAMRevised.Add(StartZ);
                                    ColumnxERAMRevised.Add(EndX);
                                    ColumnyERAMRevised.Add(EndY);
                                    ColumnzERAMRevised.Add(EndZ);
                                    ColumnNameRAMRevised.Add(ColumnNameRAM[i]);
                                    ColumnRotationRAMRevised.Add(ColumnRotationRAM[i]);
                                    IndexSplice.Add(j);
                                    found = true;
                                    break;
                                }
                            }
                        }

                        //For - Else Statement
                        if (!found)
                        {
                            Indicator = 1;
                            ColumnxSRAMRevised.Add(StartX);
                            ColumnySRAMRevised.Add(StartY);
                            ColumnzSRAMRevised.Add(StartZ);
                            ColumnxERAMRevised.Add(EndX);
                            ColumnyERAMRevised.Add(EndY);
                            ColumnzERAMRevised.Add(EndZ);
                            ColumnNameRAMRevised.Add(ColumnNameRAM[i]);
                            ColumnRotationRAMRevised.Add(ColumnRotationRAM[i]);
                        }

                    }
                }
                //If the column has already been spliced
                if (ColumnSpliceRAM[i] == 1 && !IndexSplice.Contains(i))
                {
                    ColumnxSRAMRevised.Add(ColumnxSRAM[i]);
                    ColumnySRAMRevised.Add(ColumnySRAM[i]);
                    ColumnzSRAMRevised.Add(ColumnzSRAM[i]);
                    ColumnxERAMRevised.Add(ColumnxERAM[i]);
                    ColumnyERAMRevised.Add(ColumnyERAM[i]);
                    ColumnzERAMRevised.Add(ColumnzERAM[i]);
                    ColumnNameRAMRevised.Add(ColumnNameRAM[i]);
                    ColumnRotationRAMRevised.Add(ColumnRotationRAM[i]);
                    IndexSplice.Add(i);
                }

            }

            //Get Center Points
            for (int i = 0; i < (ColumnzERAMRevised.Count()); i++)
            {
                CCPxRAM.Add((ColumnxSRAMRevised[i] + ColumnxERAMRevised[i]) * (0.5));
                CCPyRAM.Add((ColumnySRAMRevised[i] + ColumnyERAMRevised[i]) * (0.5));
                CCPzRAM.Add((ColumnzSRAMRevised[i] + ColumnzERAMRevised[i]) * (0.5));
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

            //Get Coordinates
            double OriginBeamRevitCPx = OriginBeamMidpoint.X;
            double OriginBeamRevitCPy = OriginBeamMidpoint.Y;
            double OriginBeamRevitCPz = OriginBeamMidpoint.Z;
            found = false;
            double OriginBeamRAMCPx = 0;
            double OriginBeamRAMCPy = 0;
            double OriginBeamRAMCPz = 0;

            //Get that location for Origin Beam in RAM
            for (int i = 0; i < (BeamIDRAM.Count()); i++)
            {
                if (BeamStoryRAM[i] == RAMStoryUser)
                {
                    if (BeamIDRAM[i].ToString() == RAMIDUser)
                    {
                        OriginBeamRAMCPx = (BeamxSRAM[i] + BeamxERAM[i]) * 0.5;
                        OriginBeamRAMCPy = (BeamySRAM[i] + BeamyERAM[i]) * 0.5;
                        OriginBeamRAMCPz = (BeamzSRAM[i] + BeamzERAM[i]) * 0.5;
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

            #region Translate Beam and Column Geometry

            //Translate geometry
            double deltaX = OriginBeamRevitCPx- OriginBeamRAMCPx;
            double deltaY = OriginBeamRevitCPy - OriginBeamRAMCPy;
            double deltaZ = OriginBeamRevitCPz - OriginBeamRAMCPz;

            //Rotation
            //Convert string to int
            double Rotationdouble =Convert.ToDouble(RotationUser);

            //CenterPoints for the beams
            List<double> BeamxCPRAM = new List<double>();
            List<double> BeamyCPRAM = new List<double>();
            List<double> BeamzCPRAM = new List<double>();

            //Translated geometry for the beams and the columns
            List<double> BeamxCPRAMtranslated = new List<double>();
            List<double> BeamyCPRAMtranslated = new List<double>();
            List<double> BeamzCPRAMtranslated = new List<double>();

            List<double> ColumnxCPRAMtranslated = new List<double>();
            List<double> ColumnyCPRAMtranslated = new List<double>();
            List<double> ColumnzCPRAMtranslated = new List<double>();

            //Convert Angle to radians
            double angle = Rotationdouble;

            //Beam Translation
            for (int i = 0; i < (BeamIDRAM.Count()); i++)
            {
                //Centerpoints for the beams
                BeamxCPRAM.Add((BeamxSRAM[i] + BeamxERAM[i]) * 0.5);
                BeamyCPRAM.Add((BeamySRAM[i] + BeamyERAM[i]) * 0.5);
                BeamzCPRAM.Add((BeamzSRAM[i] + BeamzERAM[i]) * 0.5);

                //Translate the beam geometry
                double translatedxbeam = BeamxCPRAM[i] + deltaX;
                double translatedybeam = BeamyCPRAM[i] + deltaY;
                double translatedzbeam = BeamzCPRAM[i] + deltaZ;

                //Local Distance Beams
                double localxbeam = BeamxCPRAM[i] - OriginBeamRAMCPx;
                double localybeam = BeamyCPRAM[i] - OriginBeamRAMCPy;

                //Rotate the beam geometry
                double rotatedxbeam = localxbeam * Math.Cos(angle) - localybeam * Math.Sin(angle);
                double rotatedybeam = localxbeam * Math.Sin(angle) + localybeam * Math.Cos(angle);

                //Finalize translated geometry for beams
                BeamxCPRAMtranslated.Add(rotatedxbeam + translatedxbeam-localxbeam);
                BeamyCPRAMtranslated.Add(rotatedybeam + translatedybeam-localybeam);
                BeamzCPRAMtranslated.Add(translatedzbeam);
            }

            //Column Translation
            for (int i = 0; i < (ColumnNameRAMRevised.Count()); i++)
            {
                //Translate the column geometry
                double translatedxcolumn = CCPxRAM[i] + deltaX;
                double translatedycolumn = CCPyRAM[i] + deltaY;
                double translatedzcolumn = CCPzRAM[i] + deltaZ;

                //Local Distance Columns
                double localxcolumn = CCPxRAM[i] - OriginBeamRAMCPx;
                double localycolumn = CCPyRAM[i] - OriginBeamRAMCPy;

                //Rotate the column geometry
                double rotatedxcolumn = localxcolumn * Math.Cos(angle) - localycolumn * Math.Sin(angle);
                double rotatedycolumn = localxcolumn * Math.Sin(angle) + localycolumn * Math.Cos(angle);

                //Finalize translated geometry for beams
                ColumnxCPRAMtranslated.Add(rotatedxcolumn + translatedxcolumn-localxcolumn);
                ColumnyCPRAMtranslated.Add(rotatedycolumn + translatedycolumn-localycolumn);
                ColumnzCPRAMtranslated.Add(translatedzcolumn);
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
                    //foreach (Element ColumnElement in ColumnElements)
                    //{
                    //    foreach (string paramname in ColParamaterNames)
                    //    {
                    //        Parameter param = ColumnElement.LookupParameter(paramname);
                    //        param.Set("");
                    //    }
                    //}



                    t.Commit();
                }
            }

            #endregion

            #region Matching Scheme
            double Tolerancedouble = Convert.ToDouble(ToleranceUser);
            List<double> MDBeams = new List<double>();
            List<int> MDBeamsI = new List<int>();
            List<double> MDColumns = new List<double>();
            List<int> MDColumnsI = new List<int>();

            using (Transaction t = new Transaction(doc, "Matching"))
            {
                t.Start("Match");
                //BEAMS
                //Start with RAM.. typically less beams in RAM Model
                for (int i = 0; i < (BeamxCPRAMtranslated.Count()); i++)
                {
                    //Check all Revit Beams
                    for (int j = 0; j < (BeamElements.Count()); j++)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((BeamxCPRAMtranslated[i] - BCPxRevit[j]), 2) + Math.Pow((BeamyCPRAMtranslated[i] - BCPyRevit[j]), 2) + Math.Pow((BeamzCPRAMtranslated[i] - BCPzRevit[j]), 2), 0.5);

                        //If the distance is less than the tolerance, store 
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDBeams.Add(Distance);
                            MDBeamsI.Add(j);
                        }
                    }

                    //If there were any matches
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
                        param.Set(BeamCamberRAM[i]);

                        //Set Studs
                        param = BeamElements[MatchI].LookupParameter(SFParamaterNames[1]);
                        param.Set(BeamStudsRAM[i]);

                        //Set Size
                        param = BeamElements[MatchI].LookupParameter(SFParamaterNames[2]);
                        param.Set(BeamNameRAM[i]);

                        //FLAG TIME BABY
                        //Camber Flag
                        //There needs to be a parser for camber to equal Rams output
                        if (BeamCamberRevit[MatchI] == BeamCamberRAM[i])
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
                        if (BeamStudsRevit[MatchI].ToUpper() == BeamStudsRAM[i].ToUpper())
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
                        if (BeamNameRevit[MatchI].ToUpper() == BeamNameRAM[i].ToUpper())
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
                for (int i = 0; i < (ColumnxCPRAMtranslated.Count()); i++)
                {
                    //Check all Revit Beams
                    for (int j = 0; j < (ColumnElements.Count()); j++)
                    {
                        //Distance Formula
                        double Distance = Math.Pow(Math.Pow((ColumnxCPRAMtranslated[i] - CCPxRevit[j]), 2) + Math.Pow((ColumnyCPRAMtranslated[i] - CCPyRevit[j]), 2) + Math.Pow((ColumnzCPRAMtranslated[i] - CCPzRevit[j]), 2), 0.5);

                        //If the distance is less than the tolerance, store 
                        if (Distance <= Tolerancedouble)
                        {
                            //Pool the matched beams in a list
                            MDColumns.Add(Distance);
                            MDColumnsI.Add(j);
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
                        param.Set(ColumnNameRAMRevised[i]);

                        //Set Orient
                        param = ColumnElements[MatchI].LookupParameter(ColParamaterNames[1]);
                        param.Set(ColumnRotationRAMRevised[i].ToString());

                        //Size Flag
                        if (ColumnNameRevit[MatchI].ToUpper() == ColumnNameRAMRevised[i].ToUpper())
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
                        if (Math.Abs(ColumnRotationRevit[MatchI] - ColumnRotationRAMRevised[i]) == 0 || Math.Abs(ColumnRotationRevit[MatchI] - ColumnRotationRAMRevised[i]) == 180)
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

                t.Commit();
            }
            #endregion

            #region colors
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

            bool exist = false;
            FilteredElementCollector all3DViews = new FilteredElementCollector(doc).OfClass(typeof(View3D));

            foreach (View3D view in all3DViews)
            {
                if (view.Name == ViewName)
                {
                    exist = true;
                }
            }

            if (exist == false)
            {
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

                //Need to add if statement for if the 3d view exists
                var collector = new FilteredElementCollector(doc);

                //ViewType
                var viewFamilyType = collector
                  .OfClass(typeof(ViewFamilyType))
                  .OfType<ViewFamilyType>()
                  .FirstOrDefault(x =>
                   x.ViewFamily == ViewFamily.ThreeDimensional);

                ElementId SolidPatternId = null;

                //This is my backup for the element filter
                List<FillPatternElement> FillPatterns = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>().ToList();

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
