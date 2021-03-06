using System.Collections.Generic;
using System.Threading.Tasks;
using MediaMarkup.Api.Models;

namespace MediaMarkup.Api
{
    public interface IApprovals
    {
        /// <summary>
        /// Gets the list of approvals for the specified parameters, <see cref="ApprovalListRequestParameters"/>
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns><see cref="ApprovalListResult"/></returns>
        Task<ApprovalListResult> GetList(ApprovalListRequestParameters parameters);

        /// <summary>
        /// Upload file to create a new Approval
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="approvalCreateParameters"></param>
        /// <returns></returns>
        Task<ApprovalCreateResult> Create(string filePath, ApprovalCreateParameters approvalCreateParameters);

        /// <summary>
        /// Gets the Approval details for the specified approval id
        /// </summary>
        /// <param name="id">Approval Id</param>
        /// <returns></returns>
        Task<Approval> Get(string id);

        /// <summary>
        /// Upload file to create a new Approval
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fileContent"></param>
        /// <param name="approvalCreateParameters"></param>
        /// <returns></returns>
        Task<ApprovalCreateResult> Create(string filename, byte[] fileContent, ApprovalCreateParameters approvalCreateParameters);

        /// <summary>
        /// Updates an approval with all the specified paramaters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Approval> Update(string id, ApprovalUpdateParameters parameters);

        /// <summary>
        /// Updates the Approval OwnerUserId
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task UpdateOwnerUserId(ApprovalUpdateOwnerUserIdParameters parameters);

        /// <summary>
        /// Updates the Approval Name / Description
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task UpdateName(ApprovalUpdateNameParameters parameters);

        /// <summary>
        /// Updates the Approval Active/Enabled property
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SetActive(ApprovalSetActiveParameters parameters);

        /// <summary>
        /// Create new version, adding a file to an existing approval
        /// </summary>
        /// <param name="filePath">Full path to file (must include a support extension)</param>
        /// <param name="parameters">Approval Create Version Parameters see <see cref="ApprovalCreateVersionParameters"/></param>
        /// <returns><see cref="ApprovalCreateVersionResult"/></returns>
        Task<ApprovalCreateVersionResult> CreateVersion(string filePath, ApprovalCreateVersionParameters parameters);

        /// <summary>
        /// Creates a new approval version
        /// </summary>
        /// <param name="filename">Filename (must include a support extension)</param>
        /// <param name="fileContent">File content as byte array</param>
        /// <param name="parameters">Approval Create Version Parameters see <see cref="ApprovalCreateVersionParameters"/></param>
        /// <returns><see cref="ApprovalCreateVersionResult"/></returns>
        Task<ApprovalCreateVersionResult> CreateVersion(string filename, byte[] fileContent, ApprovalCreateVersionParameters parameters);

        /// <summary>
        /// Deletes a version of approval
        /// </summary>
        /// <param name="approvalId">Approval ID</param>
        /// <param name="version">Version Number</param>
        /// <returns></returns>
        Task<bool> DeleteVersion(string approvalId, int version);

        /// <summary>
        /// Creates the personal url for the specified user, approvalId & version (specify compare approval id and vesion to view comparison)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns><see cref="PersonalUrlCreateResult"/></returns>
        Task<PersonalUrlCreateResult> CreatePersonalUrl(PersonalUrlCreateParameters parameters);

        /// <summary>
        /// Deletes the specified approval.
        /// All approval data is permanently deleted. This cannot be undone.
        /// Users are not deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string id);

        /// <summary>
        /// Adds a new approval group to the specified approval
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ApprovalGroupCreateResult> AddApprovalGroup(ApprovalGroupCreateParameters parameters);

        Task<Approval> CreateApprovalGroups(CreateApprovalGroupsParameters parameters);

        /// <summary>
        /// Updates existing approval group of the specified approval
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<bool> UpdateApprovalGroup(ApprovalGroupUpdateParameters parameters);

        /// <summary>
        /// Adds an approval group user to the specified approval version
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task UpsertApprovalGroupUser(ApprovalGroupUserParameters parameters);

        /// <summary>
        /// Adds multiple approval group users to the specified approval version
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task AddApprovalGroupUsers(ApprovalGroupUsersParameters parameters);

        /// <summary>
        /// Updates the specified approval group user for the specified approval version
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task UpdateApprovalGroupUser(ApprovalGroupUserParameters parameters);

        /// <summary>
        /// Removes the specified approval group user for the specified approval version
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task RemoveApprovalGroupUser(ApprovalGroupRemoveUserParameters parameters);

        /// <summary>
        /// Resets all Approval Group User Decisions and unlocks the version
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task ResetAllApprovalGroupUserDecisions(ApprovalGroupUserParameters parameters);

        /// <summary>
        /// Sets an approval group user decision
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SetApprovalGroupUserDecision(ApprovalGroupUserDecisionParameters parameters);

        /// <summary>
        /// Reset user's decision in of the given approval group
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task ResetApprovalGroupUserDecision(ApprovalGroupUserParameters parameters);

        /// <summary>
        /// Sets the approval version as locked or unlocked
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SetApprovalVersionLock(ApprovalVersionLockParameters parameters);

        /// <summary>
        /// Set Approval Group Enabled
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SetApprovalGroupEnabled(ApprovalGroupSetEnabledParameters parameters);

        /// <summary>
        /// Set Approval Group Readonly
        /// </summary>
        /// <param name="parameters"></param>
        Task SetApprovalGroupReadonly(ApprovalGroupSetReadOnlyParameters parameters);

        /// <summary>
        /// Downloads the exported annotaions, notes comments for an approval version related to image or pdf based approvals
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<byte[]> ExportAnnotationReport(ExportReportParameters parameters);
        
        /// <summary>
        /// Get Approval Draft details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApprovalDraft> GetApprovalDraftByIdAsync(string id);

        /// <summary>
        /// Create New Approval Draft
        /// </summary>
        /// <returns></returns>
        Task<ApprovalDraft> CreateApprovalDraftAsync();

        /// <summary>
        /// Delete Approval Draft
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteApprovalDraftAsync(string id);

        /// <summary>
        /// Upload New File to Approval Draft
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index">Index of the file uploaded to the draft</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<ApprovalDraft> UploadFileToApprovalDraftAsync(string id, int index, string filePath);
        
        /// <summary>
        /// Upload Multiple Files to Approval Draft
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        Task<ApprovalDraft> UploadMultipleFilesToApprovalDraftAsync(string id, string[] filePaths);

        /// <summary>
        /// Delete File from Approval Draft
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        Task<ApprovalDraft> DeleteFileFromApprovalDraftAsync(string id, string fileId);

        /// <summary>
        /// Convert Approval Draft into Approval
        /// This is a long running operation!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ApprovalCreateResult> PublishDraftAsync(string id, ApprovalCreateParameters parameters);
    }
}