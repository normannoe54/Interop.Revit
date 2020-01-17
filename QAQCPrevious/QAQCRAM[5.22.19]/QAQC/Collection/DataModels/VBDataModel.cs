#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace QAQCRAM
{
    public class VBDataModel
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

        #endregion

        #region constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VBDataModel()
        {
        }

        #endregion
    }
}
