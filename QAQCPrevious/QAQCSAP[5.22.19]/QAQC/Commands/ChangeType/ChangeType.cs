#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
#endregion

namespace QAQCSAP
{
    /// <summary>
    /// Change Type property
    /// </summary>
    public static class ChangeType
    {
        /// <summary>
        /// Change the beam type, this needs to occur inside a transaction
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="beam"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static bool changeBeamType(Document doc, FamilyInstance beam, string typename)
        {
            bool output = false;

            //Collect all Structural Framing
            FilteredElementCollector fscollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming);

            //Send to List
            List<Element> fsList = fscollector.ToList();

            //Cycle through structural framing and find beams with specific parameter
            foreach (Element Element in fsList)
            {
                if (Element.Name.ToString().ToUpper() == typename.ToUpper())
                {
                    FamilySymbol fs = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming).Cast<FamilySymbol>().FirstOrDefault(q => q.Name == Element.Name) as FamilySymbol;
                    //set beam symbol
                    beam.Symbol = fs;
                    output = true;
                    break;
                }
                else
                {
                    output = false;
                }
            }      
            return output;
        }

        /// <summary>
        /// Change the column type, needs to occur inside a transaction
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="column"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static bool changeColumnType(Document doc, FamilyInstance column, string typename)
        {
            bool output = true;

            //Collect all Structural Framing
            FilteredElementCollector fscollector = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);

            //Send to List
            List<Element> fsList = fscollector.ToList();

            //Cycle through structural framing and find beams with specific parameter
            foreach (Element Element in fsList)
            {
                if (Element.Name.ToString().ToUpper() == typename.ToUpper())
                {
                    FamilySymbol fs = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns).Cast<FamilySymbol>().FirstOrDefault(q => q.Name == Element.Name) as FamilySymbol;
                    //set beam symbol
                    column.Symbol = fs;
                    output = true;
                    break;
                }
                else
                {
                    output = false;
                }
            }
            
            return output;
        }
    }
}
