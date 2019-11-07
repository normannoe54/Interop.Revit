#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace QAQCSAP
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventSelection : IExternalEventHandler
    {
        /// <summary>
        /// Instance of userform to write to
        /// </summary>
        public QAQCForm qaqcform;

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

            //Structural Framing Reference
            Reference selectionReference = uidoc.Selection.PickObject(ObjectType.Element, new SelectionFilterByCategory("Structural Framing"), "Select Elements");

            //Select Element
            Element selectionElement = doc.GetElement(selectionReference);
            int ElementIDName = selectionElement.Id.IntegerValue;

            //Set the textbox in the userform to the elementID
            qaqcform.ElementIDText.Text = ElementIDName.ToString();
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