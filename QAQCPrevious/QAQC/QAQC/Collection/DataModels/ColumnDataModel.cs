#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace QAQC
{
    public class ColumnDataModel
    {
        #region public methods

        /// <summary>
        /// X value
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// Y value
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Z value
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// Name of beam
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Rotation value
        /// </summary>
        public double rotation { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ColumnDataModel()
        {
        }

        #endregion

    }
}
