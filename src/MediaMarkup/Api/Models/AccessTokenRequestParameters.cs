using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    public class AccessTokenRequestParameters
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientSecret")]
        public string SecretKey { get; set; }
    }
}
