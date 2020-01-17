#region Namespaces
using Autodesk.Revit.UI;
using System.Collections.Generic;
using Autodesk.Windows;
#endregion

namespace QAQCSAP
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
            string tabName = "Interoperability";
            string panelName = "SAP2000";


            //Try and catch block to create the tab
            bool tabexists = false;

            //Collection of tabs
            Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;

            //See if the tab exists in the project
            foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
            {
                if (tab.Title == tabName)
                {
                    tabexists = true;
                    break;
                }
            }
            //Create the tab if it doesn't exist
            if (tabexists == false)
            {
                try
                {
                    app.CreateRibbonTab(tabName);
                }
                catch
                { }
            }

            //Initialize Panel
            Autodesk.Revit.UI.RibbonPanel QAQCCommandsPanel = null;

            //Create the tab if it doesn't exist
            if (IsPanelCreated(tabName,panelName,app) == false)
            {
                // Create the ribbon panels.
                QAQCCommandsPanel = app.CreateRibbonPanel(tabName, panelName);
            }
            else
            {
                QAQCCommandsPanel = GetPanel(tabName, panelName, app);
            }

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

        /// <summary>
        /// Is the panel created in Revit
        /// </summary>
        /// <param name="paneltitle"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        private bool IsPanelCreated(string tabname, string paneltitle,UIControlledApplication app)
        {
            // Create ribbon tab.
            List<Autodesk.Revit.UI.RibbonPanel> loadedPluginPanels = app.GetRibbonPanels(tabname);

            foreach (Autodesk.Revit.UI.RibbonPanel p in loadedPluginPanels)
            {
                if (p.Title==paneltitle)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the Panel
        /// </summary>
        /// <param name="tabname"></param>
        /// <param name="paneltitle"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        private Autodesk.Revit.UI.RibbonPanel GetPanel(string tabName, string panelName, UIControlledApplication app)
        {
            Autodesk.Revit.UI.RibbonPanel Panel = null;

            // Create ribbon tab.
            List<Autodesk.Revit.UI.RibbonPanel> loadedPluginPanels = app.GetRibbonPanels(tabName);

            if (IsPanelCreated(tabName, panelName, app) == false)
            {
                return Panel;
            }
            else
            {
                foreach (Autodesk.Revit.UI.RibbonPanel p in loadedPluginPanels)
                {
                    if (p.Title.Equals(panelName))
                    {
                        return p;
                    }
                }
            }

            return Panel;
        }


        #endregion

    }
}
