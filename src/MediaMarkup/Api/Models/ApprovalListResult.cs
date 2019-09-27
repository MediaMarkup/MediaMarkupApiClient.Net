using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Approval Create Result
    /// </summary>
    public class ApprovalListResult
    {
        /// <summary>
        /// Approval Id
        /// </summary>
        [Obsolete("Please use Filter field", true)]
        public string Id { get; set; }

        /// <summary>
        /// Viewerer Url
        /// </summary>
        [Obsolete("Please use Filter field", true)]
        public int TotalCount { get; set; }

        [JsonProperty("filter")]
        public ApprovalFilter Filter { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [JsonProperty("approvals")]
        public List<Approval> Approvals { get; set; }
    }

    public class ApprovalFilter
    {
        private int _limit;
        private int _currentPage;

        public int Limit
        {
            get => _limit;
            set => _limit = value > 0 && value < 50 ? value : 10;
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value > 0 && value < TotalPages ? value : 1;
        }

        public int TotalPages
        {
            get => (int)(Math.Ceiling((double)TotalResult / Limit));
        }

        public int TotalResult { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SortBy SortBy { get; set; } = SortBy.Name;

        [JsonConverter(typeof(StringEnumConverter))]
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;

        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; } = Status.All;

        public string SearchTerm { get; set; }

        public string OwnerId { get; set; }

        public ApprovalFilter()
        {
            Limit = 10;
            CurrentPage = 1;
        }
    }

    public enum SortBy
    {
        Name,
        Filename,
        LastUpdated,
        Status
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }

    public enum Status
    {
        All,
        Pending,
        Approved,
        NotApproved
    }
}
