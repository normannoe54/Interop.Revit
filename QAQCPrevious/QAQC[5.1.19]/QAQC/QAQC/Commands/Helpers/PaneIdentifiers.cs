#region Namespaces
using System;
#endregion

namespace QAQC
{

    /// <summary>
    /// Guid container that holds guid references to dockable panes.
    /// </summary>
    public static class PaneIdentifiers
    {
        #region public methods

        /// <summary>
        /// The family manager dockable pane identifier.
        /// </summary>
        /// <returns></returns>
        public static Guid GetManagerPaneIdentifier()
        {
            return new Guid("74720072-53e8-43df-a914-f440dad14249");
        }

        #endregion
    }
}
