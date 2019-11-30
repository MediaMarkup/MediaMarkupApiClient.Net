using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaMarkup.Api.Models
{
    /// <summary>
    /// User Creation Parameters
    /// </summary>
    public class UserCreateParameters
    {
        /// <summary>
        /// User Create Parameters
        /// </summary>
        public UserCreateParameters()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = string.Empty;
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
        /// Email Address
        /// </summary>
        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password (min 6 characteres)
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }

        /// <summary>
        /// Specifies the User Role, Administrator, Manager, Reviewer
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Enables Login via mediamarkup.com Portal
        /// Otherwise user can only access via PURL created using API integration
        /// </summary>
        [JsonProperty("webLoginEnabled")]
        public bool WebLoginEnabled { get; set; }
    }
}
