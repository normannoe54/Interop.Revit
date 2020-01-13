#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace RAMSSWrapper
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

        /// <summary>
        /// Axial Load
        /// </summary>
        public double P { get; set; }

        /// <summary>
        /// Shear Value
        /// </summary>
        public double V { get; set; }

        /// <summary>
        /// Major Moment
        /// </summary>
        public double M3 { get; set; }

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

