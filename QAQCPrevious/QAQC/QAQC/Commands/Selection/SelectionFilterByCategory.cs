#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace QAQC
{
    /// <summary>
    /// Defines a selection filter for different elements in Revit
    /// </summary>
    public class SelectionFilterByCategory : ISelectionFilter
    {
        #region private members

        //private variable that holds category name
        private string mCategory = "";

        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// Initializes a new instance of the <see cref="SelectionFilterByCategory"/>
        /// </summary>
        /// <param name="category"></param>
        public SelectionFilterByCategory(string category)
        {
            mCategory = category;
        }
        #endregion

        #region public methods

        /// <summary>
        /// Allows the element selection if provided category is equal to the selected element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool AllowElement(Element element)
        {
            //Check if category name matches
            if (element.Category.Name == mCategory)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows the reference
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }

        #endregion
    }
}
