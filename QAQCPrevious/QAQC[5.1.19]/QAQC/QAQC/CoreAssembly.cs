#region Namespaces
using System.Reflection;
#endregion

namespace QAQC
{
    /// <summary>
    /// core assembly location
    /// </summary>
    public class CoreAssembly
    {
        #region public methods

        /// <summary>
        /// Gets the core assembly location.
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        #endregion
    }
}