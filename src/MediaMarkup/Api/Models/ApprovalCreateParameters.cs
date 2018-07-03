﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MediaMarkup.Api.Models
{
    public class ApprovalCreateParameters
    {
        /// <summary>
        /// ApprovalsCreateParameters
        /// </summary>
        public ApprovalCreateParameters()
        {
            OwnerUserId = string.Empty;
            Name = string.Empty;
            NumberOfDecisionsRequired = 0;
        }

        /// <summary>
        /// The owner User id asociated with the approval
        /// </summary>
        [JsonProperty("ownerUserId")]
        public string OwnerUserId { get; set; }

        /// <summary>
        /// Name / Description of the approval
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Number of decisions required, 0 = all users in initial approval group
        /// </summary>
        [JsonProperty("numberOfDecisionsRequired")]
        public int? NumberOfDecisionsRequired { get; set; }

        /// <summary>
        /// Deadline date for the initial approval group
        /// </summary>
        [JsonProperty("deadline")]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Adds Reviewers to the approval (these reviewers are added to the initial approval group)
        /// Owner options can be overwritten by adding the owner with specified options.
        /// All users with valid user id's (that exist) willbe added to the approval.
        /// </summary>
        [JsonProperty("reviewers")]
        public List<ApprovalGroupUser> Reviewers { get; set; }
    }
}
