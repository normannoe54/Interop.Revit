using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RAMSSWrapper
{

    /// <summary>
    /// Json TopLevel formatting
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// Steel beams
        /// </summary>
        [JsonProperty("RAMStlBeams")]
        public List<BeamDataModel> _RAMStlBeams { get; set; }

        /// <summary>
        /// Joist
        /// </summary>
        [JsonProperty("RAMJoists")]
        public List<BeamDataModel> _RAMJoists { get; set; }

        /// <summary>
        /// Columns
        /// </summary>
        [JsonProperty("RAMColumns")]
        public List<ColumnDataModel> _RAMColumns { get; set; }

        /// <summary>
        /// Vertical Braces
        /// </summary>
        [JsonProperty("RAMVBs")]
        public List<VBDataModel> _RAMVBs { get; set; }

        /// <summary>
        /// Constructor for payload
        /// </summary>
        /// <param name="RAMStlBeams"></param>
        /// <param name="RAMJoists"></param>
        /// <param name="RAMColumns"></param>
        /// <param name="RAMVBs"></param>
        public Payload(List<BeamDataModel> RAMStlBeams, List<BeamDataModel> RAMJoists, List<ColumnDataModel> RAMColumns, List<VBDataModel> RAMVBs)
        {
            _RAMStlBeams = RAMStlBeams;
            _RAMJoists = RAMJoists;
            _RAMColumns = RAMColumns;
            _RAMVBs = RAMVBs;
        }

    }
}
