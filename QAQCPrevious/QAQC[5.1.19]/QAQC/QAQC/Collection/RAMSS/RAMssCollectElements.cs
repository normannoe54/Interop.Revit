#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RAMDATAACCESSLib;
#endregion

namespace QAQC
{
    public class RAMssCollectElements
    {
        //Variables imported and exported during the process
        internal SCoordinate StartPointC { get; set; }
        internal SCoordinate EndPointC { get; set; }
        internal SCoordinate StartPointB { get; set; }
        internal SCoordinate EndPointB { get; set; }
        internal int Size { get; set; }
        internal object NumberofStuds { get; set; }

        #region Initialize RAM SS
        /// <summary>
        /// Open the RAM Interface
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public IModel InitializeRAM(string Openfile)
        {
            //Create instance of RAM DATA ACCESS
            RamDataAccess1 RAM = new RamDataAccess1();

            //Initiate IDBIO1 Interface
            IDBIO1 RAMIDBIO1 = RAM.GetDispInterfacePointerByEnum(EINTERFACES.IDBIO1_INT);

            //Load Model Data from a file name (OpenFile)
            double LOADDB = RAMIDBIO1.LoadDataBase2(Openfile, "DA");
       
            //Load the model data
            IModel Imodel = RAMIDBIO1.GetDispInterfacePointerByEnum(EINTERFACES.IModel_INT);

            return Imodel;
        }
        #endregion

        #region Get RAM Stories
        /// <summary>
        /// Collect RAM Stories
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public IStories GetStoriesRAM(IModel Imodel)
        {
            //Get Story Data
            IStories Istories = Imodel.GetStories();

            return Istories;
        }
        #endregion
    }
}
