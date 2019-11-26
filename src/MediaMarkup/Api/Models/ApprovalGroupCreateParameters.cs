using System;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Parameters for creating an approval group
    /// </summary>
    public class ApprovalGroupCreateParameters
    {
        /// <summary>
        /// Approval Id
        /// </summary>
        [JsonProperty("approvalId")]
        public string ApprovalId { get; set; }

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
