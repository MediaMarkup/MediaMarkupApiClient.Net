using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Approval Group Reviewer
    /// </summary>
    public class ApprovalGroupUser
    {
        /// <summary>
        /// Approval Group Reviewer
        /// </summary>
        public ApprovalGroupUser()
        {
            UserId = "";
            CommentsEnabled = false;
            AllowDecision = false;
            AllowDownload = false;
            AllowVersionSelection = false;
            AllowReportDownload = false; 
            Enabled = false;
        }

        /// <summary>
        /// User Id
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

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
        /// Allows version selection, otherwise only the latest version will be displayed for the user
        /// </summary>
        [JsonProperty(PropertyName = "versionSelection")]
        public bool AllowVersionSelection { get; set; }

        /// <summary>
        /// Enabled property
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }
}
