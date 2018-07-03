﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MediaMarkup.Api.Models;
using MediaMarkup.Core;

namespace MediaMarkup.Api
{
    public class Approvals : IApprovals
    {
        public HttpClient ApiClient { get; set; }

        public Approvals(HttpClient apiClient)
        {
            ApiClient = apiClient;
        }

        /// <summary>
        /// Gets the list of approvals for the specified parameters, <see cref="ApprovalListRequestParameters"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns><see cref="ApprovalListResult"/></returns>
        public async Task<ApprovalListResult> GetList(ApprovalListRequestParameters parameters)
        {
            var response = await ApiClient.GetAsync($"Approvals/GetList/?{parameters.ToQueryStringValues()}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalListResult>();
            }

            throw new ApiException("Approvals.GetList", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public async Task<ApprovalCreateResult> Create(string filePath, ApprovalCreateParameters parameters)
        {
            var filename = Path.GetFileName(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            return await Create(filename, fileContent, parameters);
        }

        /// <summary>
        /// Creates an approval, uploading the supplied media for the specified approval parameters
        /// </summary>
        /// <param name="filename">Filename of file on disk</param>
        /// <param name="fileContent">file content as a byte array</param>
        /// <param name="parameters">Create parameters, see <see cref="ApprovalCreateParameters"/></param>
        /// <returns></returns>
        public async Task<ApprovalCreateResult> Create(string filename, byte[] fileContent, ApprovalCreateParameters parameters)
        {
            // Basic client side validation to prevent unnesessary round trips on basic errors

            var fileExtension = Path.GetExtension(filename);

            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new ArgumentException("Approvals.Create: File must have an extension.");
            }

            if (fileContent.Length <= 0)
            {
                throw new ArgumentException("Approvals.Create: File length muct be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(parameters.OwnerUserId))
            {
                throw new ArgumentException("Approvals.Create: Owner not specified");
            }

            if (string.IsNullOrWhiteSpace(parameters.Name))
            {
                throw new ArgumentException("Approvals.Create: Approval name not specified");
            }

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(new MemoryStream(fileContent)), "file", HttpUtility.UrlEncode(filename));

                var values = new[]
                {
                    new KeyValuePair<string, string>("name", parameters.Name),
                    new KeyValuePair<string, string>("ownerUserId", parameters.OwnerUserId),
                    new KeyValuePair<string, string>("numberOfDecisionsRequired", (parameters.NumberOfDecisionsRequired ?? 0).ToString()),
                    new KeyValuePair<string, string>("deadline", parameters.Deadline?.ToString("O") ?? ""),
                    new KeyValuePair<string, string>("reviewers", parameters.Reviewers.ToJson())
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), $"\"{keyValuePair.Key}\"");
                }

                var response = await ApiClient.PostAsync("Approvals/Create/", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalCreateResult>();
                }

                throw new ApiException("Approvals.Create", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task Update(ApprovalUpdateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/Update/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.Update", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task UpdateOwnerUserId(ApprovalUpdateOwnerUserIdParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/UpdateOwnerUserId/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.UpdateOwnerUserId", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task UpdateName(ApprovalUpdateNameParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/UpdateName/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.UpdateName", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetActive(ApprovalSetActiveParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetActive/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetActive", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task<ApprovalCreateVersionResult> CreateVersion(string filePath, ApprovalCreateVersionParameters parameters)
        {
            var filename = Path.GetFileName(filePath);

            var fileContent = File.ReadAllBytes(filePath);

            return await CreateVersion(filename, fileContent, parameters);
        }

        /// <inheritdoc />
        public async Task<ApprovalCreateVersionResult> CreateVersion(string filename, byte[] fileContent, ApprovalCreateVersionParameters parameters)
        {
            // Basic client side validation to prevent unnesessary round trips on basic errors

            if (String.IsNullOrWhiteSpace(parameters.ApprovalId))
            {
                throw new ArgumentException("Approvals.CreateVersion: Approval Id not specified.");
            }

            var fileExtension = Path.GetExtension(filename);

            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                throw new ArgumentException("Approvals.CreateVersion: File must have an extension.");
            }

            if (fileContent.Length <= 0)
            {
                throw new ArgumentException("Approvals.CreateVersion: File length muct be greater than zero");
            }

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(new MemoryStream(fileContent)), "file", HttpUtility.UrlEncode(filename));

                var values = new[]
                {
                    new KeyValuePair<string, string>("approvalId", parameters.ApprovalId),
                    new KeyValuePair<string, string>("lockPreviousVersion", parameters.LockPreviousVersion.ToString().ToLower()),
                    new KeyValuePair<string, string>("copyApprovalGroups", parameters.CopyApprovalGroups.ToString().ToLower())
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), $"\"{keyValuePair.Key}\"");
                }

                var response = await ApiClient.PostAsync("Approvals/CreateVersion/", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalCreateVersionResult>();
                }

                throw new ApiException("Approvals.CreateVersion", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<string> GetPersonalUrl(string userId, string approvalId, int? version = null, int? compareVersion = null)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/GetPersonalUrl/", new Models.PersonalUrlRequestParameters
            {
                UserId = userId,
                ApprovalId = approvalId
            });

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new Exception($"{response.StatusCode},{response.ReasonPhrase}");
        }

        /// <inheritdoc />
        public async Task Delete(string id)
        {
            var response = await ApiClient.DeleteAsync($"Approvals/Delete/?id={id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}