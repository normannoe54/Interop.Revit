#region Namespaces
using Autodesk.Revit.UI;
#endregion

namespace QAQCSAP
{
    /// <summary>
    /// Display helper messages
    /// </summary>
    public static class Message
    {
        #region public methods

        /// <summary>
        /// Displays the specified message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void Display(string message, WindowType type)
        {
            string title = "";
            var icon = TaskDialogIcon.TaskDialogIconNone;

            switch (type)
            {
                case WindowType.Information:
                    title = "INFORMATION";
                    icon = TaskDialogIcon.TaskDialogIconInformation;
                    break;
                case WindowType.Warning:
                    title = "WARNING";
                    icon = TaskDialogIcon.TaskDialogIconWarning;
                    break;
                case WindowType.Error:
                    title = "ERROR";
                    icon = TaskDialogIcon.TaskDialogIconError;
                    break;
                default:
                    break;
            }
            //construct window to display specified messages
            var window = new TaskDialog(title)
            {
                MainContent = message,
                MainIcon = icon,
                CommonButtons = TaskDialogCommonButtons.Ok
            };

            window.Show();
        }

        #endregion

    }
}