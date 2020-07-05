using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Create multiple approval groups with users
    /// </summary>
    public class CreateApprovalGroupsParameters
    {
        /// <summary>
        /// Approval ID
        /// </summary>
        public string ApprovalId { get; set; }
        
        /// <summary>
        /// Approval Version
        /// </summary>
        public int ApprovalVersion { get; set; }
        
        /// <summary>
        /// Approvals Groups to be created
        /// </summary>
        public List<CreateApprovalGroupBatchItem> ApprovalGroups { get; set; }
    }

    public class CreateApprovalGroupBatchItem
    {
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
        
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Users added to the approval group
        /// </summary>
        [JsonProperty("users")]
        public List<ApprovalGroupUser> Users { get; set; }
    }
}