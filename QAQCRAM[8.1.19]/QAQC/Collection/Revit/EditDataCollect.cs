using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

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
                        if((ColumnElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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

                            };

                            Columns.Add(Column);
                        }
                    }

                    //If flag orient is the parameter
                    if (param == "Flag.ColumnOrient")
                    {
                        if ((ColumnElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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
            //Retrieve beams
            IList<Element> BeamElements = RevitCollectElement.SFElements(doc);

            //Iterate Beam Elements and set to data model
            foreach (Element BeamElement in BeamElements)
            {
                foreach (string param in FlagParameterNames)
                {
                    //If column size is the issue
                    if (param == "Flag.BeamSize")
                    {
                        if ((BeamElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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

                            };

                            Beams.Add(Beam);
                        }
                    }

                    //If column size is the issue
                    if (param == "Flag.Camber")
                    {

                        if ((BeamElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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

                            };

                            Beams.Add(Beam);
                        }
                    }

                    //If column size is the issue
                    if (param == "Flag.Studs")
                    {
                        if ((BeamElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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
                        if ((VBElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
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

                            };

                            VBs.Add(Beam);
                        }
                    }
                }
            }

            return VBs;
        }

    }
}

       