using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// Api User Info object
    /// </summary>
    public class User
    {
        /// <summary>
        /// Api User Info object
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// User Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// User First Name
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// User Last Name
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// User EmailAddress
        /// </summary>
        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// User Role
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Web Login Enabled
        /// </summary>
        [JsonProperty("loginEnabled")]
        public bool WebLoginEnabled { get; set; }

        /// <summary>
        /// Account Owner
        /// </summary>
        [JsonProperty("accountOwner")]
        public bool AccountOwner { get => Role == UserRole.Owner; }
    }
}