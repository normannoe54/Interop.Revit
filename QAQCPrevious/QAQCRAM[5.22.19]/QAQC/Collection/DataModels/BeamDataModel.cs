#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace QAQCRAM
{
    public class BeamDataModel
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
        /// Camber value
        /// </summary>
        public string camber { get; set; }

        /// <summary>
        /// Studs value
        /// </summary>
        public string studs { get; set; }

        /// <summary>
        /// Studs value
        /// </summary>
        public string story { get; set; }

        /// <summary>
        /// Studs value
        /// </summary>
        public string ID { get; set; }
        #endregion

        #region constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeamDataModel()
        {
        }

        #endregion
    }
}

