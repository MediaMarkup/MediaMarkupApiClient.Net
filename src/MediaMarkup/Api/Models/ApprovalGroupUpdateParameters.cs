using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaMarkup.Api.Models
{
    public class ApprovalGroupUpdateParameters
    {
        /// <summary>
        /// Approval Id
        /// </summary>
        [JsonProperty("approvalId")]
        public string ApprovalId { get; set; }

        /// <summary>
        /// Approval Group Id
        /// </summary>
        [JsonProperty("approvaGrouplId")]
        public string ApprovalGroupId { get; set; }

        /// <summary>
        /// Approval Version
        /// </summary>
        [JsonProperty("approvalVersion")]
        public int Version { get; set; }

        /// <summary>
        /// Approval Group Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Number of decisions required, 0 = all users in group
        /// </summary>
        [JsonProperty("numberOfDecisionsRequired")]
        public int? NumberOfDecisionsRequired { get; set; }

        /// <summary>
        /// Deadline date for the group approval
        /// </summary>
        [JsonProperty("deadlineDate")]
        public DateTime? Deadline { get; set; }
    }
}
