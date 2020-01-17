#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
#endregion

namespace QAQC
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventUpdate : IExternalEventHandler
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
                    string RAMValue = row.SubItems[5].Text;

                    //Get Element
                    Element Element = doc.GetElement(ElementID);

                    if (ElementType == "Beam")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance beam = Element as FamilyInstance;
                            bool output = ChangeType.changeBeamType(doc, beam, RAMValue);

                            if (output == false)
                            {
                                Message.Display("Family not loaded", WindowType.Warning);
                                break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.BeamSize");
                                param.Set("True");
                            }
                        }
                        if (Concern == "Studs")
                        {
                            Parameter param = Element.get_Parameter(BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS);
                            param.Set(RAMValue);
                            param = Element.LookupParameter("Flag.Studs");
                            param.Set("True");
                        }
                        if (Concern == "Camber")
                        {
                            Parameter param = Element.get_Parameter(BuiltInParameter.STRUCTURAL_CAMBER);
                            param.Set(RAMValue);
                            param = Element.LookupParameter("Flag.Camber");
                            param.Set("True");
                        }
                    }

                    if (ElementType == "Column")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance column = Element as FamilyInstance;
                            bool output = ChangeType.changeColumnType(doc, column, RAMValue);

                            if (output == false)
                            {
                                Message.Display("Family not loaded", WindowType.Warning);
                                break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.ColumnSize");
                                param.Set("True");
                            }
                        }
                        if (Concern == "Rotation")
                        {
                            Parameter param = Element.LookupParameter("Flag.ColumnOrient");
                            param.Set("True");
                            ColumnData.SetColumnRotation(Element, Convert.ToDouble(RAMValue));
                        }
                    }

                    if (ElementType == "VB")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance beam = Element as FamilyInstance;
                            bool output = ChangeType.changeBeamType(doc, beam, RAMValue);

                            if (output == false)
                            {
                                Message.Display("Family not loaded", WindowType.Warning);
                                break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.VBSize");
                                param.Set("True");
                            }
                        }
                    }
                    //remove the row
                    //row.Remove();
                }
                t.Commit();
                qaqcedit.UpdateTable();
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