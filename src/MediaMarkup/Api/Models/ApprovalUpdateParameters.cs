using System;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    public class ApprovalUpdateParameters
    {
        /// <summary>
        /// Approval Owner User Id
        /// </summary>
        [JsonProperty("ownerUserId")]
        public string OwnerUserId { get; set; }

        /// <summary>
        /// Name / Description of the approval
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Sets the Approval Active/Enable
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
