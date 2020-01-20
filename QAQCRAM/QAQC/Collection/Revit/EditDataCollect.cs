using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RAMSSWrapper;

namespace QAQCRAM
{
    public static class EditDataCollect
    {
        /// <summary>
        /// Collect Element Information
        /// </summary>
        public static List<EditDataModel> RecordColumnElements(Document doc)
        {

            //Initialize Beam Model
            List<EditDataModel> Columns = new List<EditDataModel>();

            //Collect Structural Framing Parameters
            List<string> ColParamaterNames = InternalConstants.StructuralColumnParameters();

            //initiate flag list
            List<string> FlagParameterNames = new List<string>();

            //Get all the flags
            foreach (string parameter in ColParamaterNames)
            {
                if (parameter.Contains("Flag."))
                {
                    FlagParameterNames.Add(parameter);
                }
            }
            //Retrieve beams
            IList<Element> ColumnElements = RevitCollectElement.ColElements(doc);

            //Iterate Beam Elements and set to data model
            foreach (Element ColumnElement in ColumnElements)
            {
                foreach (string param in FlagParameterNames)
                {
                    //If column size is the issue
                    if (param == "Flag.ColumnSize")
                    {
                        Parameter colparam = ColumnElement.LookupParameter(param);
                        string colparamval = colparam.AsString();
                        if (colparamval?.ToUpper() == "FALSE")
                        {
                            //Define EditDataModel
                            var Column = new EditDataModel
                            {
                                elementtype = "Column",
                                elementId = ColumnElement.Id.ToString(),
                                name = ColumnElement.Name,
                                concern = "Size",
                                RevitValue = ColumnElement.Name,
                                RAMValue = ColumnElement.LookupParameter("RAM.ColumnSize").AsString(),
                                RAMStory = ColumnElement.LookupParameter("RAM.ColStory").AsString(),
                            };

                            Columns.Add(Column);
                        }

                    }

                    //If flag orient is the parameter
                    if (param == "Flag.ColumnOrient")
                    {
                        Parameter colparam = ColumnElement.LookupParameter(param);
                        string colparamval = colparam.AsString();

                        if ((colparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Column = new EditDataModel
                            {
                                elementtype = "Column",
                                elementId = ColumnElement.Id.ToString(),
                                name = ColumnElement.Name,
                                concern = "Rotation",
                                RevitValue = ColumnData.ColumnRotation(ColumnElement).ToString(),
                                RAMValue = ColumnElement.LookupParameter("RAM.ColumnOrient").AsString(),
                                RAMStory = ColumnElement.LookupParameter("RAM.ColStory").AsString(),

                            };

                            Columns.Add(Column);
                        }

                    }
                }
            }

            return Columns;
        }

        /// <summary>
        /// Collect Element Information
        /// </summary>
        public static List<EditDataModel> RecordBeamElements(Document doc)
        {
            //Initialize Beam Model
            List<EditDataModel> Beams = new List<EditDataModel>();

            //Collect Structural Framing Parameters
            List<string> SFParamaterNames = InternalConstants.StructuralFramingParameters();

            //initiate flag list
            List<string> FlagParameterNames = new List<string>();

            //Get all the flags
            foreach (string parameter in SFParamaterNames)
            {
                if (parameter.Contains("Flag."))
                {
                    FlagParameterNames.Add(parameter);
                }
            }

            IList<Element> BeamElements = RevitCollectElement.SFElements(doc);

            //Iterate Beam Elements and set to data model
            foreach (Element BeamElement in BeamElements)
            {
                foreach (string param in FlagParameterNames)
                {

                    //If column size is the issue
                    if (param == "Flag.BeamSize")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Size",
                                RevitValue = BeamElement.Name,
                                RAMValue = BeamElement.LookupParameter("RAM.BeamSize").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }

                    //If column size is the issue
                    if (param == "Flag.Camber")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Camber",
                                RevitValue = BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER).AsString(),
                                RAMValue = BeamElement.LookupParameter("RAM.Camber").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }

                    //If column size is the issue
                    if (param == "Flag.Studs")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Studs",
                                RevitValue = BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS).AsString(),
                                RAMValue = BeamElement.LookupParameter("RAM.Studs").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }
                }
            }

            return Beams;
        }       

        /// <summary>
        /// Collect Element Information
        /// </summary>
        public static List<EditDataModel> RecordVBElements(Document doc)
        {
            //Initialize Beam Model
            List<EditDataModel> VBs = new List<EditDataModel>();

            //Collection Vertical Braces Parameters
            List<string> VBParamaterNames = InternalConstants.StructuralVBParameters();

            //initiate flag list
            List<string> FlagParameterNames = new List<string>();

            //Get all the flags
            foreach (string parameter in VBParamaterNames)
            {
                if (parameter.Contains("Flag."))
                {
                    FlagParameterNames.Add(parameter);
                }
            }
            //Retrieve beams
            IList<Element> VBElements = RevitCollectElement.VBElements(doc);

            //Iterate Beam Elements and set to data model
            foreach (Element VBElement in VBElements)
            {
                foreach (string param in FlagParameterNames)
                {
                    //If column size is the issue
                    if (param == "Flag.VBSize")
                    {
                        Parameter vbparam = VBElement.LookupParameter(param);
                        string vbparamval = vbparam.AsString();

                        if ((vbparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "VB",
                                elementId = VBElement.Id.ToString(),
                                name = VBElement.Name,
                                concern = "Size",
                                RevitValue = VBElement.Name,
                                RAMValue = VBElement.LookupParameter("RAM.VBSize").AsString(),
                                RAMStory = VBElement.LookupParameter("RAM.VBStory").AsString(),

                            };

                            VBs.Add(Beam);
                        }
                    }
                }
            }

            return VBs;
        }

        /// <summary>
        /// Collect Element Information
        /// </summary>
        public static List<EditDataModel> RecordSelectedBeamElements(UIDocument uidoc)
        {
            Document doc = uidoc.Document;

            //Initialize Beam Model
            List<EditDataModel> Beams = new List<EditDataModel>();

            //Collect Structural Framing Parameters
            List<string> SFParamaterNames = InternalConstants.StructuralFramingParameters();

            //initiate flag list
            List<string> FlagParameterNames = new List<string>();

            //Get all the flags
            foreach (string parameter in SFParamaterNames)
            {
                if (parameter.Contains("Flag."))
                {
                    FlagParameterNames.Add(parameter);
                }
            }

            // Get the element selection of current document.
            Selection selection = uidoc.Selection;

            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            IList<Element> SelectedElements = new List<Element>();

            //Get Elements
            foreach (ElementId elemID in selectedIds)
            {
                SelectedElements.Add(doc.GetElement(elemID));
            }

            //Initialize List
            IList<Element> BeamElements = new List<Element>();

            //Specify Param
            BuiltInParameter Camberparam = BuiltInParameter.STRUCTURAL_CAMBER;


            //Cycle through structural framing and find beams with specific parameter
            foreach (Element BeamElement in SelectedElements)
            {
                Parameter BeamCamberParam = BeamElement.get_Parameter(Camberparam);

                if (BeamCamberParam != null)
                {
                    BeamElements.Add(BeamElement);
                }

            }

            if (0 == BeamElements.Count)
            {
                // If no elements selected.
                Message.Display("Could not find a steel beam in the selection", WindowType.Error);
                return Beams;
            }


            //Iterate Beam Elements and set to data model
            foreach (Element BeamElement in BeamElements)
            {
                foreach (string param in FlagParameterNames)
                {

                    //If column size is the issue
                    if (param == "Flag.BeamSize")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Size",
                                RevitValue = BeamElement.Name,
                                RAMValue = BeamElement.LookupParameter("RAM.BeamSize").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }

                    //If column size is the issue
                    if (param == "Flag.Camber")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Camber",
                                RevitValue = BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER).AsString(),
                                RAMValue = BeamElement.LookupParameter("RAM.Camber").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }

                    //If column size is the issue
                    if (param == "Flag.Studs")
                    {
                        Parameter beamparam = BeamElement.LookupParameter(param);
                        string beamparamval = beamparam.AsString();

                        if ((beamparamval?.ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Beam = new EditDataModel
                            {
                                elementtype = "Beam",
                                elementId = BeamElement.Id.ToString(),
                                name = BeamElement.Name,
                                concern = "Studs",
                                RevitValue = BeamElement.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS).AsString(),
                                RAMValue = BeamElement.LookupParameter("RAM.Studs").AsString(),
                                RAMStory = BeamElement.LookupParameter("RAM.BeamStory").AsString(),

                            };

                            Beams.Add(Beam);
                        }

                    }
                }
            }

            return Beams;
        }
    }
}

