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
                //Constructors
                bool notfound = false;
                List<string> FamilyDNE = new List<string>();

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
                    string SAPValue = row.SubItems[5].Text;
                  
                    //Get Element
                    Element Element = doc.GetElement(ElementID);

                    if (ElementType == "Beam")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance beam = Element as FamilyInstance;
                            bool output = ChangeType.changeBeamType(doc, beam, SAPValue);

                            if (output == false)
                            {
                                notfound = true;
                                FamilyDNE.Add(SAPValue);
                                //Message.Display("Family not loaded", WindowType.Warning);
                                //break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.SizeSAP");
                                param.Set("True");
                            }
                        }
                    }

                    if (ElementType == "Column")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance column = Element as FamilyInstance;
                            bool output = ChangeType.changeColumnType(doc, column, SAPValue);

                            if (output == false)
                            {
                                notfound = true;
                                FamilyDNE.Add(SAPValue);
                                //Message.Display("Family not loaded", WindowType.Warning);
                                //break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.SizeSAP");
                                param.Set("True");
                            }
                        }
                    }

                    if (ElementType == "VB")
                    {
                        if (Concern == "Size")
                        {
                            FamilyInstance vb = Element as FamilyInstance;
                            bool output = ChangeType.changeBeamType(doc, vb, SAPValue);

                            if (output == false)
                            {
                                notfound = true;
                                FamilyDNE.Add(SAPValue);
                                //Message.Display("Family not loaded", WindowType.Warning);
                                //break;
                            }
                            else
                            {
                                Parameter param = Element.LookupParameter("Flag.SizeSAP");
                                param.Set("True");
                            }
                        }
                    }
                    //remove the row
                    //row.Remove();
                }
                t.Commit();

                if (notfound == true)
                {
                    string Note = "The following families are not loaded:" + Environment.NewLine;
                    foreach (string family in FamilyDNE)
                    {
                        Note = Note + family + Environment.NewLine;
                    }

                    Message.Display(Note, WindowType.Warning);
                }

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