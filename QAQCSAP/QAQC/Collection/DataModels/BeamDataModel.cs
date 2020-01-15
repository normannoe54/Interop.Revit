#region Namespace
using Autodesk.Revit.DB;
#endregion

namespace QAQCSAP
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
        /// Studs value
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Element
        /// </summary>
        public Element element { get; set; }

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

