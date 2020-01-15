#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ETABSv1;
#endregion

namespace QAQCSAP
{
    public class CollectInfo
    {
        #region Public Methods
        /// <summary>
        /// Collect Beam Elements from RAM Model
        /// </summary>
        /// <param name="Imodel"></param>
        /// <returns></returns>
        public List<BeamDataModel> GetBeams(string Filename)
        {
            bool initiate = false;
            cOAPI SapObject =null;
            try
            {
                SapObject = (cOAPI)System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.ETABS.API.ETABSObject");

            }
            catch
            {
            }

            int ret = 0;
            if (SapObject ==null)
            {
                cHelper myHelper;
                
                myHelper = new Helper();
                
                string pathToETABS = @"C:\Program Files\Computers and Structures\ETABS 18\ETABS.exe";
                SapObject = myHelper.CreateObject(pathToETABS);

                //start SAP2000 application
                ret = SapObject.ApplicationStart();
                initiate = true;
            }

            //create SapModel object
            cSapModel SapModel;
            SapModel = SapObject.SapModel;

            //open an existing file
            ret = SapModel.File.OpenFile(Filename);

            //Constructors
            List<BeamDataModel> SAPBeams = new List<BeamDataModel>();
            //bool jointtoggle = false;
            //bool Sectiontoggle = false;
            //bool Locationtoggle = false;

            List<int> FrameIDJoint = new List<int>();
            List<int> JointI = new List<int>();
            List<int> JointJ = new List<int>();
            List<int> FrameIDSection = new List<int>();
            List<string> SectionName = new List<string>();
            List<double> xloc = new List<double>();
            List<double> yloc = new List<double>();
            List<double> zloc = new List<double>();
            List<int> JointID = new List<int>();

            int NumberNames = 0;
            string[] MyName = null;

            List<string>FrameNames = new List<string>();

            ret = SapModel.FrameObj.GetNameList(ref NumberNames, ref MyName);
            List<string> ElementName = MyName.ToList();        

            List<string>StartPoint = new List<string>();
            List<string> EndPoint = new List<string>();
            string point1 = "";
            string point2 = "";
            double x1 = 0;
            double x2 = 0;
            double y1 = 0;
            double y2 = 0;
            double z1 = 0;
            double z2 = 0;
            string PropName = "";
            string SAuto = "";

            //Collect data for output
            foreach (string elementName in ElementName)
            {
                //Retrieve points and section
                ret = SapModel.FrameObj.GetPoints(elementName, ref point1, ref point2);
                ret = SapModel.FrameObj.GetSection(elementName, ref PropName, ref SAuto);

                //Get Point Locations
                ret = SapModel.PointObj.GetCoordCartesian(point1, ref x1, ref y1, ref z1);
                ret = SapModel.PointObj.GetCoordCartesian(point2, ref x2, ref y2, ref z2);

                //Create beammodels
                var SAPBeam = new BeamDataModel
                {
                    x = (x1 + x2) / (2 * 12),
                    y = (y1 + y2) / (2 * 12),
                    z = (z1 + z2) / (2 * 12),
                    name = PropName,
                    ID = elementName,
                };

                SAPBeams.Add(SAPBeam);

            }

            //close SAP2000
            if (initiate)
            {
                SapObject.ApplicationExit(false);
            }

            SapModel = null;
            SapObject = null;


            return SAPBeams;
        }
      
        #endregion

    }
}
