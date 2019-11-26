using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    public class UserInvitation
    {
        [JsonProperty("invitationUrl")]
        public string Url { get; set; }
    }
}
