using System.Collections.Generic;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Approval Version Status Info with Status based on the built-in approval rules
    /// </summary>
    public class ApprovalGroupStatusInfo
    {
        /// <summary>
        /// Creates an instance of the Approval Version Status Info based on built-in approval rules 
        /// </summary>
        public ApprovalGroupStatusInfo()
        {
        }

        /// <summary>
        /// Approval Group Name
        /// </summary>
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// Number of decisions requied for group
        /// </summary>
        [JsonProperty("decisionsRequired")]
        public int DecisionsRequired { get; set; }

        /// <summary>
        /// Number of decision makers in group
        /// </summary>
        [JsonProperty("decisionMakers")]
        public int DecisionMakers { get; set; }
        
        /// <summary>
        /// Number of decisions Approved
        /// </summary>
        [JsonProperty("decisionsApproved")]
        public int DecisionsApproved { get; set; }

        /// <summary>
        /// Number of decisions Not Approved
        /// </summary>
        [JsonProperty("decisionsNotApproved")]
        public int DecisionsNotAprroved { get; set; }

        /// <summary>
        /// Overall status for group
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Users in group who have approved file
        /// </summary>
        [JsonProperty("approvedByUsers")]
        public List<string> ApprovedByUsers { get; set; }

        /// <summary>
        /// Users in group with no decisions on file
        /// </summary>
        [JsonProperty("noDecisionsByUsers")]
        public List<string> NoDecisionsByUsers { get; set; }

        /// <summary>
        /// Uusers in group who have not approved file
        /// </summary>
        [JsonProperty("notApprovedByUsers")]
        public List<string> NotApprovedByUsers { get; set; }
    }
}
