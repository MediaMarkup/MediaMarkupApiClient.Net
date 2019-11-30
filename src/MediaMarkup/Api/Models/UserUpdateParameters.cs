using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// User Update Parameters
    /// </summary>
    public class UserUpdateParameters
    {
        /// <summary>
        /// User Update Parameters
        /// </summary>
        public UserUpdateParameters()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// First Name
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Enables Login via mediamarkup.com Portal
        /// Otherwise user can only access via PURL created using API integration
        /// </summary>
        [JsonProperty("webLoginEnabled")]
        public bool WebLoginEnabled { get; set; }
    }
}
