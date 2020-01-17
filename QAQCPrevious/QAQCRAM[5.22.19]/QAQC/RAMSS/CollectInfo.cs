﻿#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAMDATAACCESSLib;
#endregion

namespace QAQCRAM
{
    public class CollectInfo
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

        #region Public Methods
        /// <summary>
        /// Collect Beam Elements from RAM Model
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public List<BeamDataModel> GetBeams(IModel Imodel)
        {
            //Initialize Beam Model
            List<BeamDataModel> RAMBeams = new List<BeamDataModel>();

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

                //Get all the beams at the specified story
                IBeams Ibeams = Istory.GetBeams();

                //Filer Steel Beams
                Ibeams.Filter(EBeamFilter.eBeamFilter_Material, EMATERIALTYPES.ESteelMat);
                    //(EBeamFilter.eBeamFilter_Material, EMATERIALTYPES.ESteelMat && EMATERIALTYPES.ESteelJoistMat);

                //Determine number of beams
                int NumBeams = Ibeams.GetCount();

                //Collect information on the multiple columns at that specific level
                for (int Beamint = 0; Beamint < (NumBeams); Beamint++)
                {
                    //Get each individual columns
                    IBeam Ibeam = Ibeams.GetAt(Beamint);

                    //Get Start and End Coordinates
                    SCoordinate StartPointB = new SCoordinate();
                    SCoordinate EndPointB = new SCoordinate();
                    EBeamCoordLoc eBeamCoordLoc = new EBeamCoordLoc();
                    Ibeam.GetCoordinates(eBeamCoordLoc, ref StartPointB, ref EndPointB);

                    //Determine camber
                    double camber = Ibeam.dCamber;
                    string camberstring = camber.ToString();

                    //Change data
                    string camberfracstring = SimpleRefine.Decimaltofractions(camberstring);

                    //Determine studs
                    ISteelBeamDesignResult SteelBeamDesign = Ibeam.GetSteelDesignResult();
                    DAArray numStudsDAArray = SteelBeamDesign.GetNumStudsInSegments();
                    string strNumberofStuds = "";
                    List<string> StudArray = new List<string>();

                    string BeamStudsRAM = "0";

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

                        BeamStudsRAM = (String.Join(";", StudArray.ToArray()));
                        StudArray.Clear();
                    }
                    else
                    {
                        BeamStudsRAM = "0";
                    }

                    var RAMBeam = new BeamDataModel
                    {
                        x = (StartPointB.dXLoc + EndPointB.dXLoc) / (2 * 12),
                        y = (StartPointB.dYLoc + EndPointB.dYLoc) / (2 * 12),
                        z = (StartPointB.dZLoc + EndPointB.dZLoc) / (2 * 12),
                        name = Ibeam.strSectionLabel,
                        studs = SimpleRefine.StringTrimmer(BeamStudsRAM),
                        camber = camberfracstring,
                        story = Istory.strLabel,
                        ID = Ibeam.lLabel.ToString()
                };

                    RAMBeams.Add(RAMBeam);
                }
            }

            return RAMBeams;
        }

        /// <summary>
        /// Collect RAM Columns, incorporate splice correction
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public List<ColumnDataModel> GetColumns(IModel Imodel)
        {
            //Initialize Beam Model
            List<ColumnDataModel> RAMColumns = new List<ColumnDataModel>();

            #region Constructors
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
            bool found = false;
            #endregion

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
                Icolumns.Filter(EColumnFilter.eColFilter_Material, EMATERIALTYPES.ESteelMat);

                //Determine number of columns
                int NumColumns = Icolumns.GetCount();

                //Collect information on the multiple columns at that specific level
                for (int Column = 0; Column < (NumColumns); Column++)
                {
                    //Get each individual columns
                    IColumn Icolumn = Icolumns.GetAt(Column);

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
                   
                    double RotationAdjusted = Math.Round(Icolumn.dOrientation);
                    //double RotationAdjusted = Math.Round(Icolumn.dOrientation);

                    //Theres a 90 degree difference in the global coordinates from RAM and Revit
                    RotationAdjusted = Math.Abs(RotationAdjusted - 90);
                    ColumnRotationRAM.Add(RotationAdjusted);

                    //Check if this is a splice Level
                    ColumnSpliceRAM.Add(Icolumn.bSpliceLevel);
                }
            }

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

                    while (Indicator != 1)
                    {
                        //allows for a for - else statement
                        found = false;

                        for (int j = 0; j < (ColumnIDRAM.Count()); j++)
                        {

                            //Does the endpoint match the startpoint
                            if ((Math.Round(ColumnxSRAM[j], 3) == Math.Round(EndX, 3)) && (Math.Round(ColumnySRAM[j], 3) == Math.Round(EndY, 3)) && (Math.Round(ColumnzSRAM[j], 3) == Math.Round(EndZ, 3)) && !IndexSplice.Contains(j))
                            {
                                //If the column continues up the height of the building
                                if (ColumnSpliceRAM[j] == 0)
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

                var RAMColumn = new ColumnDataModel
                {
                    x = (ColumnxSRAMRevised[i] + ColumnxERAMRevised[i]) * (0.5),
                    y = (ColumnySRAMRevised[i] + ColumnyERAMRevised[i]) * (0.5),
                    z = (ColumnzSRAMRevised[i] + ColumnzERAMRevised[i]) * (0.5),
                    name = ColumnNameRAMRevised[i],
                    rotation = ColumnRotationRAMRevised[i]
                };
                RAMColumns.Add(RAMColumn);
            }
            
            return RAMColumns;
        }

        /// <summary>
        /// Collect Joists
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public List<BeamDataModel> GetJoists(IModel Imodel)
        {
            //Initialize Beam Model
            List<BeamDataModel> RAMBeams = new List<BeamDataModel>();

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

                //Get all the beams at the specified story
                IBeams Ibeams = Istory.GetBeams();

                //Filer Steel Beams
                Ibeams.Filter(EBeamFilter.eBeamFilter_Material, EMATERIALTYPES.ESteelJoistMat);

                //Determine number of beams
                int NumBeams = Ibeams.GetCount();

                //Collect information on the multiple columns at that specific level
                for (int Beamint = 0; Beamint < (NumBeams); Beamint++)
                {
                    //Get each individual columns
                    IBeam Ibeam = Ibeams.GetAt(Beamint);

                    //Get Start and End Coordinates
                    SCoordinate StartPointB = new SCoordinate();
                    SCoordinate EndPointB = new SCoordinate();
                    EBeamCoordLoc eBeamCoordLoc = new EBeamCoordLoc();
                    Ibeam.GetCoordinates(eBeamCoordLoc, ref StartPointB, ref EndPointB);

                    var RAMBeam = new BeamDataModel
                    {
                        x = (StartPointB.dXLoc + EndPointB.dXLoc) / (2 * 12),
                        y = (StartPointB.dYLoc + EndPointB.dYLoc) / (2 * 12),
                        z = (StartPointB.dZLoc + EndPointB.dZLoc) / (2 * 12),
                        name = Ibeam.strSectionLabel,
                        studs = "0",
                        camber = "0",
                        story = Istory.strLabel,
                        ID = Ibeam.lLabel.ToString()
                    };

                    RAMBeams.Add(RAMBeam);
                }
            }

            return RAMBeams;
        }

        /// <summary>
        /// Collect Joists
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public List<VBDataModel> GetVB(IModel Imodel)
        {
            //Initialize Beam Model
            List<VBDataModel> RAMVBs = new List<VBDataModel>();

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

                //Get all the beams at the specified story
                IVerticalBraces IverticalBraces = Istory.GetVerticalBraces();

                //Filer Steel Beams
                IverticalBraces.Filter(EVerticalBraceFilter.eVBFilter_Material, EMATERIALTYPES.ESteelMat);

                //Determine number of beams
                int NumVBs = IverticalBraces.GetCount();

                //Collect information on the multiple columns at that specific level
                for (int VBint = 0; VBint < (NumVBs); VBint++)
                {
                    //Get each individual columns
                    IVerticalBrace IverticalBrace = IverticalBraces.GetAt(VBint);

                    //Get Start and End Coordinates
                    SCoordinate StartPointB = new SCoordinate();
                    SCoordinate EndPointB = new SCoordinate();
                    IverticalBrace.GetEndCoordinates(ref StartPointB, ref EndPointB);

                    var RAMBeam = new VBDataModel
                    {
                        x = (StartPointB.dXLoc + EndPointB.dXLoc) / (2 * 12),
                        y = (StartPointB.dYLoc + EndPointB.dYLoc) / (2 * 12),
                        z = (StartPointB.dZLoc + EndPointB.dZLoc) / (2 * 12),
                        name = IverticalBrace.strSectionLabel,
                    };

                    RAMVBs.Add(RAMBeam);
                }
            }

            return RAMVBs;
        }
        #endregion
    }
}
