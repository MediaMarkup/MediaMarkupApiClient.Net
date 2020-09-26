using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MediaMarkup.Api.Models
{
    public class ApprovalDraft
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount => Pages?.Count ?? 0;

        [JsonProperty(PropertyName = "pages")] 
        public Dictionary<double, List<ApprovalDraftFile>> Pages { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "status")]
        public ApprovalDraftStatus Status { get; set; } = ApprovalDraftStatus.Draft;
        
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
    
    public class ApprovalDraftFile
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        
        [JsonProperty(PropertyName = "index")]
        public double Index { get; set; }
    }

    public enum ApprovalDraftStatus
    {
        Draft,
        Processing,
        Failed,
        Published
    }
}