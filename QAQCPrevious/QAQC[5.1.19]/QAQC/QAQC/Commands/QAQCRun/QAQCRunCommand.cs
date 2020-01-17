#region Namespaces
using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace QAQC
{
    ///  <summary>
    ///  QAQC Run Command code to be executed when button is clicked.
    ///  </summary>
    ///  <seealso cref="Autodesk.Revit.UI.IExternalApplication"/>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class QAQCRunCommand : IExternalCommand
    {
        #region public methods
        /// <summary>
        /// Execute when QAQCRun button is clicked
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // A new handler to handle request posting by the dialog
            ExternalEventRun handler = new ExternalEventRun();
             
            // External Event for the dialog to use (to post requests)
            ExternalEvent Event = ExternalEvent.Create(handler);            

            //Application Context
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Check if we are in the Revit project, not in the family editor.
            if (doc.IsFamilyDocument)
            {
                Message.Display("Can't use command in family document", WindowType.Warning);
                return Result.Cancelled;
            }

            try
            {
                //Initiate userform with event handler
                QAQCForm myQAQCForm = new QAQCForm(Event,handler);
                handler.qaqcform = myQAQCForm;

                //******************************************************************************** WE MIGHT NOT NEED THIS
                myQAQCForm.uidoc = uidoc;
                myQAQCForm.doc = doc;
                myQAQCForm.app = app;

                //Show Modeless Userform
                myQAQCForm.Show();
              
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        public static string GetPath()
        {
            // Return constructed namespace path.
            return typeof(QAQCRunCommand).Namespace + "." + nameof(QAQCRunCommand);
        }
        #endregion
    }
}
