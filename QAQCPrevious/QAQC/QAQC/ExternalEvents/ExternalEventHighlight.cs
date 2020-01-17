#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Forms;
#endregion

namespace QAQC
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventHighlight : IExternalEventHandler
    {
        /// <summary>
        /// Instance of userform to write to
        /// </summary>
        public QAQCEdit qaqcedit;

        /// <summary>
        /// Execute Event
        /// </summary>
        /// <param name="uiapp"></param>
        public void Execute(UIApplication uiapp)
        {
            //Define Document
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Initiate List
            IList<ElementId> ElementID = new List<ElementId>();

            //Collect Data
            //Get Selected Rows from List View
            foreach (ListViewItem row in qaqcedit.LVDataList.SelectedItems)
            {
                string ElementIDUser = row.SubItems[1].Text;
                int OriginElementIDint = 0;
                Int32.TryParse(ElementIDUser, out OriginElementIDint);
                ElementId OriginElementId = new ElementId(OriginElementIDint);
                ElementID.Add(OriginElementId);
            }

            //Select Elements
            uidoc.Selection.SetElementIds(ElementID);
        }

        /// <summary>
        /// Predefined Name function needed
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "Structural Framing Select";
        }
    }
}