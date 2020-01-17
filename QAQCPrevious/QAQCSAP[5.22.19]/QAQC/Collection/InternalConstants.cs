using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAQCSAP
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
            SFParamaterNames.Add("Analysis.SizeSAP");
            SFParamaterNames.Add("Flag.SizeSAP");

            return SFParamaterNames;
        }

    }
}
