#region Namespaces
using System;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.Diagnostics;
#endregion

namespace QAQC
{
    public class ExternalEventRunApp : IExternalApplication
    {
        // class instance
        //public static ExternalEventExampleApp thisApp = null;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {           
            //thisApp = this;  // static access to this application in//stance

            return Result.Succeeded;
        }

        //   The external command invokes this on the end-user's request
        public static void RunCommand(UIApplication uiapp)
        {
            // A new handler to handle request posting by the dialog
            ExternalEventRun handler = new ExternalEventRun();

            // External Event for the dialog to use (to post requests)
            ExternalEvent Event = ExternalEvent.Create(handler);

        }
    }
}