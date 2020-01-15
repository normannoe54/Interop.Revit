using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace QAQCSAP
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
            List<string> ColParamaterNames = InternalConstants.StructuralFramingParameters();

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
                    if (param == "Flag.SizeSAP")
                    {
                        if ((ColumnElement.LookupParameter(param).AsString().ToUpper() == "FALSE"))
                        {
                            //Define EditDataModel
                            var Column = new EditDataModel
                            {
                                elementtype = "Column",
                                elementId = ColumnElement.Id.ToString(),
                                name = ColumnElement.Name,
                                concern = "Size",
                                RevitValue = ColumnElement.Name,
                                RAMValue = ColumnElement.LookupParameter("Analysis.SizeSAP").AsString(),

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
                    if (param == "Flag.SizeSAP")
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
                                RAMValue = BeamElement.LookupParameter("Analysis.SizeSAP").AsString(),

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
            List<string> VBParamaterNames = InternalConstants.StructuralFramingParameters();

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
                    if (param == "Flag.SizeSAP")
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
                                RAMValue = VBElement.LookupParameter("Analysis.SizeSAP").AsString(),

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

