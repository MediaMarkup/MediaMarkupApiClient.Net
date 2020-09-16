
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// ApprovalGroupUserParameters for adding and updating approval group users
    /// </summary>
    public class ApprovalGroupUserParameters
    {
        /// <summary>
        /// Approval Group Reviewer
        /// </summary>
        public ApprovalGroupUserParameters()
        {
            Id = "";
            UserId = "";
            ApprovalGroupId = "";
            CommentsEnabled = false;
            AllowDecision = false;
            AllowDownload = false;
            AllowReportDownload = false;
        }

        /// <summary>
        /// The Approval Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Approval Version
        /// </summary>
        [JsonProperty("approvalVersion")]
        public int Version { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Approval Group Id specifies the approval group Id to add the user to.<br />
        /// If approvalGroupId is empty or null and the appproval version only has 1 group, then the user will be added to that group
        /// </summary>
        [JsonProperty("approvalGroupId")]
        public string ApprovalGroupId { get; set; }

        //[JsonProperty("")]
        //public bool Administrator { get; set; }

        /// <summary>
        /// Enables commenting on approval
        /// </summary>
        [JsonProperty("commentsEnabled")]
        public bool CommentsEnabled { get; set; }

        /// <summary>
        /// Enables making a decision (approving/not approving a file)
        /// </summary>
        [JsonProperty("allowDecision")]
        public bool AllowDecision { get; set; }

        /// <summary>
        /// Allows the user to download the file for approval
        /// </summary>
        [JsonProperty("allowDownload")]
        public bool AllowDownload { get; set; }

        /// <summary>
        /// Allows the user to download the approval report
        /// </summary>
        [JsonProperty("allowReportDownload")]
        public bool AllowReportDownload { get; set; }

        /// <summary>
        /// Enables/Disables user
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }
}
