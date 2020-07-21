using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    public class ApprovalGroup
    {
        public ApprovalGroup()
        {
            NumberOfDecisionsRequired = 0;
            Enabled = false;
            Users = new List<ApprovalGroupUser>();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Number of decisions reqiured 0 = all
        /// </summary>
        [JsonProperty(PropertyName = "numberOfDecisionsRequired")]
        public int NumberOfDecisionsRequired { get; set; }

        [JsonProperty(PropertyName = "deadline")]
        public DateTime? DeadlineDate { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        [JsonProperty(PropertyName = "users")]
        public List<ApprovalGroupUser> Users { get; set; }

        [JsonProperty(PropertyName = "approvalGroupStatusInfo")]
        public ApprovalGroupStatusInfo ApprovalGroupStatusInfo { get; set; }

    }
}
