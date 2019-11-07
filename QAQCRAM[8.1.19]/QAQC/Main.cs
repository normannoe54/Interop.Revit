#region Namespaces
using System;
using Autodesk.Revit.UI;
#endregion

namespace QAQCRAM
{
    /// <summary>
    /// Pugin's main entry point.
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalApplication" />
    public class Main : IExternalApplication
    {
        #region external application public methods

        /// <summary>
        /// Called when Revit starts up.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            //Initialize whole plugin's user interface
            var ui = new SetupInterface();
            ui.Initialize(application);

            //Applicationclosing event
            application.ApplicationClosing += a_ApplicationClosing;

            //Set Application to Idling
            application.Idling += a_Idling;

            return Result.Succeeded;
        }

        /// <summary>
        /// Idling command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void a_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {

        }
        /// <summary>
        /// Closing command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void a_ApplicationClosing(object sender, Autodesk.Revit.UI.Events.ApplicationClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shutdown command
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
        #endregion
    }
}
