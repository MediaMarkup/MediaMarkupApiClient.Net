
using System;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Approval List Request Parameters
    /// </summary>
    public class ApprovalListRequestParameters
    {
        /// <summary>
        /// Approval List Request Parameters
        /// </summary>
        public ApprovalListRequestParameters()
        {
            Page = 1;
            ResultsPerPage = 50;
            TextFilter = string.Empty;
            UserIdFilter = string.Empty;
            SortBy = SortBy.LastUpdated;
            SortDirection = SortDirection.Desc;
            Status = Status.All;
        }

        /// <summary>
        /// Page, startiung from 1
        /// </summary>
        [JsonProperty("currentPage")]
        public int Page { get; set; }

        /// <summary>
        /// Maximum rows per page
        /// </summary>
        [JsonProperty("limit")]
        public int? ResultsPerPage { get; set; }

        /// <summary>
        /// Text Filter (basic full phrase search on text fields)
        /// </summary>
        [JsonProperty("searchTerm")]
        public string TextFilter { get; set; }

        /// <summary>
        /// Approval Owner Id Filter to ensure result only contains approvals with the specified owner id.
        /// </summary>
        [JsonProperty("ownerIdFilter")]
        [Obsolete("Please use UserIdFilter", true)]
        public string OwnerIdFilter { get; set; }

        /// <summary>
        /// User Id Filter to ensure result only contains approvals visible to the enabled specified user.
        /// </summary>
        [JsonProperty("ownerId")]
        public string UserIdFilter { get; set; }

        /// <summary>
        /// SortBy
        /// Valid property names: name, latestFilename, approvalStatus, latestDeadline, active, lastUpdated
        /// 
        /// </summary>
        [JsonProperty("sortBy")]
        public SortBy SortBy { get; set; }

        /// <summary>
        /// Sort Order Direction
        /// </summary>
        [JsonProperty("sortDirection")]
        public SortDirection SortDirection { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}
