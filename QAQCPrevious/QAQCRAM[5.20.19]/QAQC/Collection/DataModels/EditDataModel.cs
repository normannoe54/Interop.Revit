
namespace QAQCRAM
{
    public class EditDataModel
    {
        #region public methods

        /// <summary>
        /// ElementType
        /// </summary>
        public string elementtype { get; set; }

        /// <summary>
        /// ElementId
        /// </summary>
        public string elementId { get; set; }

        /// <summary>
        /// Beam name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Description of concern
        /// </summary>
        public string concern { get; set; }

        /// <summary>
        /// Revit value
        /// </summary>
        public string RevitValue { get; set; }

        /// <summary>
        /// RAM value
        /// </summary>
        public string RAMValue { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditDataModel()
        {
        }

        #endregion
    }
}
