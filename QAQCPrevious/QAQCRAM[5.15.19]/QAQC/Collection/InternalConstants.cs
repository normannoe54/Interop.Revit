using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAQC
{
    public static class InternalConstants
    {
        /// <summary>
        /// Collection of Structural Framing Parameters
        /// </summary>
        /// <returns></returns>
        public static List<string> StructuralFramingParameters()
        {
            List<string> SFParamaterNames = new List<string>();
            SFParamaterNames.Add("RAM.Camber");
            SFParamaterNames.Add("RAM.Studs");
            SFParamaterNames.Add("RAM.BeamSize");
            SFParamaterNames.Add("Flag.Camber");
            SFParamaterNames.Add("Flag.Studs");
            SFParamaterNames.Add("Flag.BeamSize");

            return SFParamaterNames;
        }

        /// <summary>
        /// Collection of Structural Column Parameters
        /// </summary>
        /// <returns></returns>
        public static List<string> StructuralColumnParameters()
        {
            List<string> ColParamaterNames = new List<string>();
            ColParamaterNames.Add("RAM.ColumnSize");
            ColParamaterNames.Add("RAM.ColumnOrient");
            ColParamaterNames.Add("Flag.ColumnSize");
            ColParamaterNames.Add("Flag.ColumnOrient");

            return ColParamaterNames;
        }

        /// <summary>
        /// Collection of Structural VB Parameters
        /// </summary>
        /// <returns></returns>
        public static List<string> StructuralVBParameters()
        {
            List<string> VBParamaterNames = new List<string>();
            VBParamaterNames.Add("RAM.VBSize");
            VBParamaterNames.Add("Flag.VBSize");

            return VBParamaterNames;
        }

    }
}
