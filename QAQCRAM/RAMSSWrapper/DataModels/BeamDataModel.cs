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
        public double PS { get; set; }

        /// <summary>
        /// Shear Major Value
        /// </summary>
        public double VMajS { get; set; }

        /// <summary>
        /// Shear Minor Value
        /// </summary>
        public double VMinS { get; set; }

        /// <summary>
        /// Major Moment
        /// </summary>
        public double MMajS { get; set; }

        /// <summary>
        /// Minor Moment
        /// </summary>
        public double MMinS { get; set; }

        /// <summary>
        /// Axial Load
        /// </summary>
        public double PE { get; set; }

        /// <summary>
        /// Shear Major Value
        /// </summary>
        public double VMajE { get; set; }

        /// <summary>
        /// Shear Minor Value
        /// </summary>
        public double VMinE { get; set; }

        /// <summary>
        /// Major Moment
        /// </summary>
        public double MMajE { get; set; }

        /// <summary>
        /// Minor Moment
        /// </summary>
        public double MMinE { get; set; }

        /// <summary>
        /// RAM Story
        /// </summary>
        public string RAMStory { get; set; }
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

