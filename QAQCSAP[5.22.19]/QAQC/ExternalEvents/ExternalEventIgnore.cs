#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
#endregion

namespace QAQCSAP
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventIgnore : IExternalEventHandler
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
        //Define Document
        { 
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Collect Data
            //Get Selected Rows from List View
            using (Transaction t = new Transaction(doc, "Update"))
            {
                t.Start("Update");

                foreach (ListViewItem row in qaqcedit.LVDataList.SelectedItems)
                {
                    string ElementIDUser = row.SubItems[1].Text;
                    int OriginElementIDint = 0;
                    Int32.TryParse(ElementIDUser, out OriginElementIDint);
                    ElementId OriginElementId = new ElementId(OriginElementIDint);
                    ElementId ElementID = OriginElementId;
                    string ElementType = row.SubItems[0].Text;
                    string Concern = row.SubItems[3].Text;

                    //Get Element
                    Element Element = doc.GetElement(ElementID);

                    if (ElementType == "Beam")
                    {
                        if (Concern == "Size")
                        {
                            Parameter param = Element.LookupParameter("Flag.SizeSAP");
                            param.Set("");
                        }
                    }

                    if (ElementType == "Column")
                    {
                        if (Concern == "Size")
                        {
                            Parameter param = Element.LookupParameter("Flag.SizeSAP");
                            param.Set("");                          
                        }
                    }

                    if (ElementType == "VB")
                    {
                        if (Concern == "Size")
                        {
                            Parameter param = Element.LookupParameter("Flag.SizeSAP");
                            param.Set("");
                        }
                    }
                    //remove the row
                    row.Remove();
                }
                t.Commit();
            }           
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