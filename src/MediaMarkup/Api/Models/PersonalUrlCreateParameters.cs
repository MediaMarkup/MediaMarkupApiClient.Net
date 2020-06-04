using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Personal Url Create Parameters
    /// </summary>
    public class PersonalUrlCreateParameters
    {
        /// <summary>
        /// UserId to authenticate and authorize
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Approval Id to review
        /// </summary>
        [JsonProperty("approvalId")]
        public string ApprovalId { get; set; }
        
        /// <summary>
        /// Approval Group Id to review
        /// </summary>
        [JsonProperty("approvalGroupId")]
        public string ApprovalGroupId { get; set; }

        /// <summary>
        /// Approval Version to review
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Approval Id to compare
        /// </summary>
        [JsonProperty("compareApprovalId")]
        public string CompareApprovalId { get; set; }

        /// <summary>
        /// Approval Version to compare
        /// </summary>
        [JsonProperty("compareVersion")]
        public int CompareVersion { get; set; }

        /// <summary>
        /// Observer / Guest User to view approval/version readonly
        /// </summary>
        [JsonProperty("observer")]
        public bool? Observer { get; set; }

        /// <summary>
        /// Allow Approval Note Moderation
        /// </summary>
        [JsonProperty("allowNoteModeration")]
        public bool AllowNoteModeration { get; set; }
    }
}
