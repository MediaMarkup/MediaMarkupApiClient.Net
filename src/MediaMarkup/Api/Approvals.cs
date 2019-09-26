using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        /// <inheritdoc />
        public async Task<Approval> Get(string id)
        {
            var response = await ApiClient.GetAsync($"approvals/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<Approval>();
            }

            throw new ApiException("Approvals.Get", response.StatusCode, await response.Content.ReadAsStringAsync());
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
                var fileFormContent = new ByteArrayContent(fileContent, 0, fileContent.Length);
                fileFormContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = filename };
                fileFormContent.Headers.ContentLength = fileContent.Length;

                content.Add(fileFormContent);

                var values = new[]
                {
                    new KeyValuePair<string, string>("name", parameters.Name),
                    new KeyValuePair<string, string>("ownerUserId", parameters.OwnerUserId),
                    new KeyValuePair<string, string>("numberOfDecisionsRequired", (parameters.NumberOfDecisionsRequired ?? 0).ToString()),
                    new KeyValuePair<string, string>("deadline", parameters.Deadline?.ToString("O") ?? "")
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), $"\"{keyValuePair.Key}\"");
                }


                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var response = await ApiClient.PostAsync("/approvals", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalCreateResult>();
                }

                throw new ApiException("Approvals.Create", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task<Approval> Update(string id, ApprovalUpdateParameters parameters)
        {
            var response = await ApiClient.PutAsJsonAsync($"/approvals/{id}", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await Get(id);
            }

            throw new ApiException("Approvals.Update", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [Obsolete("This method is obsolete. Use Update method.", true)]
        /// <inheritdoc />
        public async Task UpdateOwnerUserId(ApprovalUpdateOwnerUserIdParameters parameters)
        {
            throw new NotImplementedException("This method is obsolete.Use Update method.");
        }

        [Obsolete("This method is obsolete. Use Update method.", true)]
        /// <inheritdoc />
        public async Task UpdateName(ApprovalUpdateNameParameters parameters)
        {
            throw new NotImplementedException("This method is obsolete.Use Update method.");
        }

        [Obsolete("This method is obsolete. Use Update method.", true)]
        /// <inheritdoc />
        public async Task SetActive(ApprovalSetActiveParameters parameters)
        {
            throw new NotImplementedException("This method is obsolete.Use Update method.");
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
                content.Add(new StreamContent(new MemoryStream(fileContent)), "file", WebUtility.UrlEncode(filename));

                var values = new[]
                {
                    new KeyValuePair<string, string>("approvalId", parameters.ApprovalId),
                    new KeyValuePair<string, string>("lockPreviousVersion", parameters.LockPreviousVersion.ToString().ToLower()),
                    new KeyValuePair<string, string>("copyApprovalGroups", parameters.CopyApprovalGroups.ToString().ToLower()),
                    new KeyValuePair<string, string>("numberOfDecisionsRequired", (parameters.NumberOfDecisionsRequired ?? 0).ToString()),
                    new KeyValuePair<string, string>("addOwnerToInitialApprovalGroup", (parameters.AddOwnerToInitialApprovalGroup ?? false).ToString()),
                    new KeyValuePair<string, string>("deadline", parameters.Deadline?.ToString("O") ?? ""),
                    new KeyValuePair<string, string>("reviewers", parameters.Reviewers.ToJson())
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

        /// <inheritdoc />
        public async Task<PersonalUrlCreateResult> CreatePersonalUrl(PersonalUrlCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/CreatePersonalUrl/", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<PersonalUrlCreateResult>();
            }

            throw new ApiException("Approvals.GetPersonalUrl", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task Delete(string id)
        {
            var response = await ApiClient.DeleteAsync($"/approvals/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task<ApprovalGroupCreateResult> AddApprovalGroup(ApprovalGroupCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/AddApprovalGroup/", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalGroupCreateResult>();
            }

            throw new ApiException("Approvals.AddApprovalGroup", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task AddApprovalGroupUser(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/AddApprovalGroupUser/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.AddApprovalGroupUser", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task AddApprovalGroupUsers(ApprovalGroupUsersParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/AddApprovalGroupUsers/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.AddApprovalGroupUsers", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task UpdateApprovalGroupUser(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/UpdateApprovalGroupUser/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.UpdateApprovalGroupUser", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task RemoveApprovalGroupUser(ApprovalGroupRemoveUserParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/RemoveApprovalGroupUser/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.RemoveApprovalGroupUser", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task ResetApprovalGroupUserDecisions(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/ResetApprovalGroupUserDecisions/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.ResetApprovalGroupUserDecisions", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetApprovalGroupUserDecision(ApprovalGroupUserDecisionParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalGroupUserDecision/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalGroupUserDecision", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetApprovalVersionLock(ApprovalVersionLockParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalVersionLock/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalVersionLock", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetApprovalGroupEnabled(ApprovalGroupSetEnabledParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalGroupEnabled/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalGroupEnabled", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetApprovalGroupReadonly(ApprovalGroupSetReadOnlyParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalGroupReadonly/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalGroupReadonly", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task<byte[]> ExportAnnotationReport(ExportReportParameters parameters)
        {
            var response =  await ApiClient.GetAsync($"/approvals/{parameters.Id}/report?groupId={parameters.ApprovalGroupId}&version={parameters.Version}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            throw new ApiException("Approvals.ExportAnnotationReport", response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}