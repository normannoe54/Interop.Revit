#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SAP2000v20;
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
            cHelper helper = new Helper();
            cOAPI ApplicationObject;
            ApplicationObject = helper.GetObject("CSI.SAP2000.API.SapObject");

            //Constructors
            List<BeamDataModel> SAPBeams = new List<BeamDataModel>();
            bool jointtoggle = false;
            bool Sectiontoggle = false;
            bool Locationtoggle = false;

            List<int> FrameIDJoint = new List<int>();
            List<int> JointI = new List<int>();
            List<int> JointJ = new List<int>();
            List<int> FrameIDSection = new List<int>();
            List<string> SectionName = new List<string>();
            List<double> xloc = new List<double>();
            List<double> yloc = new List<double>();
            List<double> zloc = new List<double>();
            List<int> JointID = new List<int>();

            //Open $2k text file
            using (StreamReader file = new StreamReader(Filename))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    //Untoggle after empty line
                    if (ln == " ")
                    {
                        jointtoggle = false;
                        Sectiontoggle = false;
                        Locationtoggle = false;
                    }
                    
                    //Update Lists if toggle is true
                    //Joint Info
                    if (jointtoggle == true)
                    {
                        //FrameID, JointI, JointJ
                        string FrameIDstring = StringEditor("Frame=", "   JointI=", ln);
                        string JointIIDstring = StringEditor("JointI=", "   JointJ=", ln);
                        string JointJIDstring = StringEditor("JointJ=", "   IsCurved=", ln);
                        int FrameIDint;
                        int JointIint;
                        int JointJint;
                        Int32.TryParse(FrameIDstring, out FrameIDint);
                        Int32.TryParse(JointIIDstring, out JointIint);
                        Int32.TryParse(JointJIDstring, out JointJint);

                        //Write to List
                        FrameIDJoint.Add(FrameIDint);
                        JointI.Add(JointIint);
                        JointJ.Add(JointJint);
                    }

                    //Section Info
                    if (Sectiontoggle == true)
                    {
                        //Section Sizse
                        string FrameIDstring = StringEditor("Frame=", "   AutoSelect=", ln);
                        string SectionNamestring = StringEditor("AnalSect=", "   MatProp=", ln);
                        int FrameIDint;
                        Int32.TryParse(FrameIDstring, out FrameIDint);

                        //Write to list
                        FrameIDSection.Add(FrameIDint);
                        SectionName.Add(SectionNamestring);
                    }

                    //Location of Joints
                    if (Locationtoggle == true)
                    {
                        //Section Size
                        string JointIDstring = StringEditor("Joint=", "   CoordSys=", ln);
                        string xstring = StringEditor("XorR=", "   Y=", ln);
                        string ystring = StringEditor("Y=","   Z=", ln);
                        string zstring = StringEditor("Z=","   SpecialJt=", ln);
                        int JointIDint;
                        Int32.TryParse(JointIDstring, out JointIDint);
                        xloc.Add(Convert.ToDouble(xstring));
                        yloc.Add(Convert.ToDouble(ystring));
                        zloc.Add(Convert.ToDouble(zstring));

                        //Write to list
                        JointID.Add(JointIDint);
                    }

                    //Initiate joint toggle
                    if (ln == "TABLE:  \"CONNECTIVITY - FRAME\"")
                    {
                        jointtoggle = true;
                    }
                    //Initiate joint toggle
                    if (ln == "TABLE:  \"FRAME SECTION ASSIGNMENTS\"")
                    {
                        Sectiontoggle = true;
                    }
                    //Initiate joint toggle
                    if (ln == "TABLE:  \"JOINT COORDINATES\"")
                    {
                        Locationtoggle = true;
                    }
                }

                file.Close();
            }

            //Convert all info into BeamDataModels
            for (int i = 0; i < (FrameIDJoint.Count); i++)
            {
                //Iterate through each frame in $2k file
                int Frame = FrameIDJoint[i];

                //Get the start and end joint
                double xI = xloc[JointID.IndexOf(JointI[i])];
                double yI = yloc[JointID.IndexOf(JointI[i])];
                double zI = zloc[JointID.IndexOf(JointI[i])];

                double xJ = xloc[JointID.IndexOf(JointJ[i])];
                double yJ = yloc[JointID.IndexOf(JointJ[i])];
                double zJ = zloc[JointID.IndexOf(JointJ[i])];

                //Get the SAP Section
                string SAPSection = SectionName[(FrameIDSection.IndexOf(Frame))];

                //Create beammodels
                var SAPBeam = new BeamDataModel
                {
                    x = (xI + xJ) / (2 * 12),
                    y = (yI + yJ) / (2 * 12),
                    z = (zI + zJ) / (2 * 12),
                    name = SAPSection,
                    ID = Frame.ToString(),
                };

                SAPBeams.Add(SAPBeam);
            }
            return SAPBeams;
        }
      
        /// <summary>
        /// Collect Substring from text file
        /// </summary>
        /// <param name="firstst"></param>
        /// <param name="secondst"></param>
        /// <param name="ln"></param>
        /// <returns></returns>
        private string StringEditor(string firstst, string secondst, string ln)
        {
            int pfrom = ln.IndexOf(firstst) + firstst.Length;
            int pTo = ln.IndexOf(secondst);
            string outputst = ln.Substring(pfrom, pTo - pfrom);
            return outputst;
        }
        #endregion



    }
}
