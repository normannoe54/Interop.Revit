#region Namespaces
using Autodesk.Revit.UI;
#endregion

namespace QAQCSAP
{
    public class ExternalEventRunApp : IExternalApplication
    {
        /// <summary>
        /// On shutdown
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// On Startup
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {           
            //thisApp = this;  // static access to this application in//stance

            return Result.Succeeded;
        }

        /// <summary>
        /// The external command invokes this on the end-user's request
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RunCommand(UIApplication uiapp)
        {
            //Run handler to handle request posting by the dialog
            ExternalEventRun HandlerRun = new ExternalEventRun();

            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventRun = ExternalEvent.Create(HandlerRun);

            //Run handler to handle request posting by the dialog
            ExternalEventSelection HandlerSelection = new ExternalEventSelection();

            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventSelection = ExternalEvent.Create(HandlerSelection);

            //Initialize the form
            QAQCForm qaqcform = new QAQCForm(EventRun, HandlerRun, EventSelection, HandlerSelection);
        }
    }
}