#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
#endregion

namespace QAQC
{
    public static class RevitCollectElement
    {
        #region SF Collection
        /// <summary>
        /// Collect all beam elements in the revit project with camber parameter assigned
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<Element> SFElements(Document doc)
        {
            //Collect all Structural Framing
            FilteredElementCollector SF = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming);

            //Send to List
            IList<Element> SFelementList = SF.ToElements();

            //Initialize List
            IList<Element> SFelementListFinal = new List<Element>();

            //Specify Param
            BuiltInParameter Camberparam = BuiltInParameter.STRUCTURAL_CAMBER;

            //Cycle through structural framing and find beams with specific parameter
            foreach (Element BeamElement in SFelementList)
            {
                Parameter BeamCamberParam = BeamElement.get_Parameter(Camberparam);

                if (BeamCamberParam != null)
                {
                    SFelementListFinal.Add(BeamElement);
                }

            }

            return SFelementListFinal;
        }
        #endregion

        #region Col Collection
        /// <summary>
        /// Collect all column elements in the revit project with camber parameter assigned
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<Element> ColElements(Document doc)
        {

            //Create element collector of structural framing category
            FilteredElementCollector Col = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns);

            //IS THERE A PARAMETER THAT I CAN FILTER THE COLUMNS BY? MATERIAL == STEEL?
            ElementClassFilter filter = new ElementClassFilter(typeof(FamilyInstance));

            Col.WherePasses(filter);

            //filter out non steel beams
            IList<Element> ColelementList = Col.ToElements();

            return ColelementList;
        }
        #endregion

        #region VB Collection
        /// <summary>
        /// Collect all vertical bracing elements in the revit project
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static IList<Element> VBElements(Document doc)
        {
            //Filter out Beams with built in camber parameter
            string StructuralUsagetext = "Vertical Bracing";

            //Define Built in parameter
            BuiltInParameter StructuralUsageParam = BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM;

            //Get parameter value
            ParameterValueProvider pvpStructuralUsage = new ParameterValueProvider(new ElementId((int)StructuralUsageParam));

            //Create filter rule for element collector
            FilterStringRuleEvaluator fnrvStrStructuralUsage = new FilterStringContains();

            //Define filter with built in parameter camber and filter out beams without the parameter
            FilterStringRule paramFrStructuralUsage = new FilterStringRule(pvpStructuralUsage, fnrvStrStructuralUsage, StructuralUsagetext, false);

            //Create element collector of structural framing category
            FilteredElementCollector SF = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming);

            //Define element parameter filter
            ElementParameterFilter epfVB = new ElementParameterFilter(paramFrStructuralUsage);

            //filter out non steel beams
            IList<Element> VBelementList = SF.WherePasses(epfVB).ToElements();

            return VBelementList;
        }
        #endregion

        #region Project Parameter Collection

        /// <summary>
        /// Collect all project parameters in the revit project
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<string> ProjectParameters(Document doc)
        {
            //Define Project Parameters initialization
            BindingMap bm = doc.ParameterBindings;
            DefinitionBindingMapIterator itor = bm.ForwardIterator();
            itor.Reset();
            Definition d = null;
            List<string> ProjectParameterList = new List<string>();

            //Collect all project parameters
            while (itor.MoveNext())
            {
                d = itor.Key;
                ProjectParameterList.Add(d.Name);
            }

            return ProjectParameterList;
        }
        #endregion
    }
}
