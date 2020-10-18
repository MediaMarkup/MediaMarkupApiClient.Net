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
            var filterQuery = $"limit={parameters.ResultsPerPage}&currentPage={parameters.Page}&searchTerm={parameters.TextFilter}&ownerId={parameters.UserIdFilter}&status={parameters.Status}&sortDirection={parameters.SortDirection}&sortBy={parameters.SortBy}";
            filterQuery = WebUtility.UrlEncode(filterQuery);
            var response = await ApiClient.GetAsync($"approvals?{filterQuery}");

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

            if (string.IsNullOrWhiteSpace(parameters.Description))
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
                    new KeyValuePair<string, string>("description", parameters.Description),
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
                var fileFormContent = new ByteArrayContent(fileContent, 0, fileContent.Length);
                fileFormContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = filename };
                fileFormContent.Headers.ContentLength = fileContent.Length;

                content.Add(fileFormContent);

                var values = new[]
                {
                    new KeyValuePair<string, string>("approvalId", parameters.ApprovalId),
                    new KeyValuePair<string, string>("CopyApprovalGroups", false.ToString()),
                    new KeyValuePair<string, string>("SendNotifications", false.ToString()),
                    new KeyValuePair<string, string>("LockPreviousVersion", false.ToString())
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), $"\"{keyValuePair.Key}\"");
                }


                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var response = await ApiClient.PostAsync($"approvals/{parameters.ApprovalId}/versions", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalCreateVersionResult>();
                }

                throw new ApiException("Approvals.CreateVersion", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<bool> DeleteVersion(string approvalId, int version)
        {
            var response = await ApiClient.DeleteAsync($"/approvals/{approvalId}/versions/{version}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new ApiException("Approvals.DeleteVersion", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<PersonalUrlCreateResult> CreatePersonalUrl(PersonalUrlCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("purls/", parameters);

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
            var response = await ApiClient.PostAsJsonAsync($"/approvals/{parameters.ApprovalId}/groups", parameters);

            if (response.IsSuccessStatusCode)
            {
                var approvalGroup = await response.Content.ReadAsJsonAsync<ApprovalGroup>();

                var result = new ApprovalGroupCreateResult
                {
                    ApprovalGroupId = approvalGroup.Id,
                    Id = parameters.ApprovalId,
                    HasErrors = false,
                    Version = parameters.Version
                };

                return result;
            }

            throw new ApiException("Approvals.AddApprovalGroup", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public async Task<Approval> CreateApprovalGroups(CreateApprovalGroupsParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync($"/approvals/{parameters.ApprovalId}/groups/batch", parameters);

            if (response.IsSuccessStatusCode)
            {
                var approval = await response.Content.ReadAsJsonAsync<Approval>();
                return approval;
            }

            throw new ApiException("Approvals.CreateApprovalGroups", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<bool> UpdateApprovalGroup(ApprovalGroupUpdateParameters parameters)
        {
            var response = await ApiClient.PutAsJsonAsync($"/approvals/{parameters.ApprovalId}/groups", parameters);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new ApiException("Approvals.AddApprovalGroup", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task UpsertApprovalGroupUser(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync($"/approvals/{parameters.Id}/groups/{parameters.ApprovalGroupId}/users", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.AddApprovalGroupUser", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [Obsolete("Apiv2 doesn't support batch approval group user import. Contact support for more details.", true)]
        /// <inheritdoc />
        public async Task AddApprovalGroupUsers(ApprovalGroupUsersParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/AddApprovalGroupUsers/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.AddApprovalGroupUsers", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        [Obsolete("Please use UpsertApprovalGroupUser method", true)]
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
            var response = await ApiClient.DeleteAsync($"/approvals/{parameters.Id}/groups/{parameters.ApprovalGroupId}/users/{parameters.UserId}?version={parameters.Version}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.RemoveApprovalGroupUser", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task ResetAllApprovalGroupUserDecisions(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.DeleteAsync($"/approvals/{parameters.Id}/groups/{parameters.ApprovalGroupId}/decisions?version={parameters.Version}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.ResetApprovalGroupUserDecisions", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task ResetApprovalGroupUserDecision(ApprovalGroupUserParameters parameters)
        {
            var response = await ApiClient.DeleteAsync($"/approvals/{parameters.Id}/groups/{parameters.ApprovalGroupId}/users/{parameters.UserId}/decisions?version={parameters.Version}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.ResetApprovalGroupUserDecisions", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task SetApprovalGroupUserDecision(ApprovalGroupUserDecisionParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync($"/approvals/{parameters.Id}/groups/{parameters.ApprovalGroupId}/users/{parameters.UserId}/decisions", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalGroupUserDecision", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        [Obsolete("Please use UpdateApprovalGroup method", true)]
        public async Task SetApprovalVersionLock(ApprovalVersionLockParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalVersionLock/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalVersionLock", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        [Obsolete("Please use UpdateApprovalGroup method", true)]
        public async Task SetApprovalGroupEnabled(ApprovalGroupSetEnabledParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync("Approvals/SetApprovalGroupEnabled/", parameters);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException("Approvals.SetApprovalGroupEnabled", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        [Obsolete("Please use UpdateApprovalGroup method", true)]
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

        /// <inheritdoc />
        public async Task<ApprovalDraft> GetApprovalDraftByIdAsync(string id)
        {
            var response = await ApiClient.GetAsync($"/drafts/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalDraft>();
            }

            throw new ApiException("ApprovalDrafts.Get", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<ApprovalDraft> CreateApprovalDraftAsync()
        {
            var response = await ApiClient.PostAsync($"/drafts", null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalDraft>();
            }

            throw new ApiException("ApprovalDrafts.Post", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<bool> DeleteApprovalDraftAsync(string id)
        {
            var response = await ApiClient.DeleteAsync($"/drafts/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<bool>();
            }

            throw new ApiException("ApprovalDrafts.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<ApprovalDraft> UploadFileToApprovalDraftAsync(string id, int index, string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var fileContent = File.ReadAllBytes(filePath);

            using (var content = new MultipartFormDataContent())
            {
                var fileFormContent = new StreamContent(new MemoryStream(fileContent));
                fileFormContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = filename };
                fileFormContent.Headers.ContentLength = fileContent.Length;

                content.Add(fileFormContent);

                content.Add(fileFormContent);
                content.Add(new StringContent($"{index}"), "index");

                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var response = await ApiClient.PutAsync($"/drafts/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalDraft>();
                }

                throw new ApiException("ApprovalDrafts.Upload", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        
        /// <inheritdoc />
        public async Task<ApprovalDraft> UploadMultipleFilesToApprovalDraftAsync(string id, string[] filePaths)
        {
            using (var content = new MultipartFormDataContent())
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    var filePath = filePaths[i];
                    var filename = Path.GetFileName(filePath);
                    var fileContent = File.ReadAllBytes(filePath);
                    
                    var fileFormContent = new StreamContent(new MemoryStream(fileContent));
                    fileFormContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "files", FileName = filename };
                    fileFormContent.Headers.ContentLength = fileContent.Length;

                    content.Add(fileFormContent);
                }

                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                var response = await ApiClient.PutAsync($"/drafts/batch/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsJsonAsync<ApprovalDraft>();
                }

                throw new ApiException("ApprovalDrafts.Upload", response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        /// <inheritdoc />
        public async Task<ApprovalDraft> DeleteFileFromApprovalDraftAsync(string id, string fileId)
        {
            var response = await ApiClient.DeleteAsync($"/drafts/{id}/files/{fileId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalDraft>();
            }

            throw new ApiException("ApprovalDraftsFile.Delete", response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<ApprovalCreateResult> PublishDraftAsync(string id, ApprovalCreateParameters parameters)
        {
            var response = await ApiClient.PostAsJsonAsync($"/drafts/{id}/publish", parameters);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<ApprovalCreateResult>();
            }

            throw new ApiException("ApprovalDraftsPublish.Post", response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}