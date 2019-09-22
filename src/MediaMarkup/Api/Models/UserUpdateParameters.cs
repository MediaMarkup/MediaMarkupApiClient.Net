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
            Id = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
            WebLoginEnabled = false;
        }

        /// <summary>
        /// User Id of the User to update (required)
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

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
        /// Email Address
        /// </summary>
        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Specifies the User Role, Administrator, Manager, Reviewer
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Enables login via mediamarkup.com on tenant account
        /// </summary>
        [JsonProperty("loginEnabled")]
        public bool WebLoginEnabled { get; set; }
    }
}
