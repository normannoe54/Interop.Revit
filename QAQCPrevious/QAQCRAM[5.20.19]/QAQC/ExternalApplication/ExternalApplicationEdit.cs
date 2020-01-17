#region Namespaces
using Autodesk.Revit.UI;
#endregion

namespace QAQCRAM
{
    public class ExternalEventEditApp : IExternalApplication
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
        public static void EditCommand(UIApplication uiapp)
        {
            //Run handler to handle request posting by the dialog
            ExternalEventHighlight HandlerHighlight = new ExternalEventHighlight();

            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventHighlight = ExternalEvent.Create(HandlerHighlight);

            //Run handler to handle request posting by the dialog
            ExternalEventUpdate HandlerUpdate = new ExternalEventUpdate();

            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventUpdate = ExternalEvent.Create(HandlerUpdate);

            //Run handler to handle request posting by the dialog
            ExternalEventIgnore HandlerIgnore = new ExternalEventIgnore();

            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventIgnore = ExternalEvent.Create(HandlerIgnore);

            //Initialize the form
            QAQCEdit qaqcedit = new QAQCEdit(EventHighlight, HandlerHighlight, EventUpdate, HandlerUpdate,EventIgnore,HandlerIgnore);
        }
    }
}