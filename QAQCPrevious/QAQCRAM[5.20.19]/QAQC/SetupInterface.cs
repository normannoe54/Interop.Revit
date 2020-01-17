#region Namespaces
using Autodesk.Revit.UI;
#endregion

namespace QAQCRAM
{
    /// <summary>
    /// Setup whole plugins interface with tabs, panels, buttons,...
    /// </summary>
    public class SetupInterface
    {
        #region constructor

        /// <summary>
        /// Default constructor.
        /// Initializes a new instance of the <see cref="SetupInterface"/> class.
        /// </summary>
        public SetupInterface()
        {

        }
        #endregion

        #region public methods

        /// <summary>
        /// Initializes all interface elements on custom created Revit tab.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Initialize(UIControlledApplication app)
        {
            // Create ribbon tab.
            string tabName = "Interoperability";
            app.CreateRibbonTab(tabName);

            // Create the ribbon panels.
            var QAQCCommandsPanel = app.CreateRibbonPanel(tabName, "RAM SS");

            #region QAQC Run Button

            // Populate button data model.
            var QAQCButtonData = new RevitPushButtonDataModel
            {
                Label = "QAQC Run",
                Panel = QAQCCommandsPanel,
                Tooltip = "Use to compare RAM Structural Systems Model against Revit Model",
                CommandNamespacePath = QAQCRunCommand.GetPath(),
                IconImageName = "QAQCRun.png",
                TooltipImageName = "QAQCRun.png"
            };

            // Create button from provided data.
            var QAQCButton = RevitPushButton.Create(QAQCButtonData);

            #endregion

            #region QAQC Edit Button

            // Populate button data model.
            var QAQCEditButtonData = new RevitPushButtonDataModel
            {
                Label = "QAQC Edit",
                Panel = QAQCCommandsPanel,
                Tooltip = "Use to update Revit model after it has been checked",
                CommandNamespacePath = QAQCEditCommand.GetPath(),
                IconImageName = "QAQCEdit.png",
                TooltipImageName = "QAQCEdit.png"
            };

            // Create button from provided data.
            var QAQCEditButton = RevitPushButton.Create(QAQCEditButtonData);

            #endregion

        }

        #endregion
    }
}
