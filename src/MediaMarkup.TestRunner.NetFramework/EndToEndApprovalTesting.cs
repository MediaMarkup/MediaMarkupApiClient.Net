using MediaMarkup.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class EndToEndApprovalTesting
    {
        private readonly ApiClient _apiClient;
        private readonly TestContainer _testContainer;
        private readonly InteractiveMode _interactiveMode;

        internal EndToEndApprovalTesting(ApiClient apiClient, TestContainer testContainer, InteractiveMode interactiveMode)
        {
            _apiClient = apiClient;
            _testContainer = testContainer;
            _interactiveMode = interactiveMode;
        }

        internal async Task Run()
        {
            await CreateApproval();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await GetApproval();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await UpdateApproval();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await GetApproval();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await GetApprovalList();

            _interactiveMode.Run();

            await CreateApprovalVersion();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await CreateApprovalGroup();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await UpdateApprovalGroup();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await UpsertApprovalGroupUser();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await UpdateApprovalGroupUserDecision();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await ResetApprovalGroupUserDecision();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await UpdateApprovalGroupUserDecision();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await ResetAllApprovalGroupDecisions();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await DeleteApprovalGroupUser();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await DeleteApprovalVersion();
            Printer.PrintApproval(_testContainer.Approval);

            _interactiveMode.Run();

            await ExportApprovalReport();

            await CreatePersonalUrl();
        }

        private async Task CreateApproval()
        {
            Printer.PrintStepTitle("Uploads A Dummpy PDF For Approval");

            var random = new Random();
            var filePath = Path.Combine("dummy.pdf");

            var requestParameters = new ApprovalCreateParameters
            {
                Name = "Test Approval",
                NumberOfDecisionsRequired = random.Next(1, 10),
                OwnerUserId = _testContainer.User.Id,
            };

            Printer.Print("Creating new approval...");

            var approvalResult = await _apiClient.Approvals.Create(filePath, requestParameters);
            Printer.Print($"New approval created:{approvalResult.Id}");

            var approval = new Approval 
            {
                Id = approvalResult.Id
            };

            _testContainer.SetApproval(approval);
        }

        private async Task GetApproval()
        {
            Printer.PrintStepTitle("Get Approval Details");

            var approval = await _apiClient.Approvals.Get(_testContainer.Approval.Id);
            Printer.Print("Approval found!");

            _testContainer.SetApproval(approval);
        }

        private async Task UpdateApproval()
        {
            Printer.PrintStepTitle("Update Existing Approval");

            var parameters = new ApprovalUpdateParameters
            {
                Active = true,
                Name = "Updated Test Approval",
                OwnerUserId = _testContainer.User.Id
            };

            Printer.Print($"Updating approval...");

            var approval = await _apiClient.Approvals.Update(_testContainer.Approval.Id, parameters);
            Printer.Print("Approval updated!");

            _testContainer.SetApproval(approval);
        }

        private async Task ExportApprovalReport()
        {
            Printer.PrintStepTitle("Export Report Of An Existing Approval");

            var parameters = new ExportReportParameters
            {
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Id = _testContainer.Approval.Id,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Exporting report...");
            var result = await _apiClient.Approvals.ExportAnnotationReport(parameters);

            Printer.Print($"Saving export file ({result.Length})...");

            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var reportPath = Path.Combine(directory, $"{Guid.NewGuid().ToString().Substring(0, 5)}.pdf");

            File.WriteAllBytes(reportPath, result);
            Printer.Print($"Saved to ${reportPath}");
        }

        private async Task GetApprovalList()
        {
            Printer.PrintStepTitle("List and Filter Approvals");

            Printer.Print("Getting approvals...");

            var parameters = new ApprovalListRequestParameters
            {
                Page = 1,
                SortBy = SortBy.Status,
                SortDirection = SortDirection.Desc,
                TextFilter = _testContainer.Approval.Name,
                UserIdFilter = _testContainer.User.Id,
                Status = Status.All
            };
            var approvalListResult = await _apiClient.Approvals.GetList(parameters);
            approvalListResult.Approvals.ForEach(approval => Printer.PrintApproval(approval));
        }

        private async Task CreateApprovalVersion()
        {
            Printer.PrintStepTitle("Creates A New Version Of An Existing Approval");

            var filePath = Path.Combine("dummy.pdf");

            var parameters = new ApprovalCreateVersionParameters
            {
                ApprovalId = _testContainer.Approval.Id,
                NumberOfDecisionsRequired = 3,
            };

            Console.WriteLine("Creating new approval...");

            var approvalResult = await _apiClient.Approvals.CreateVersion(filePath, parameters);
            Printer.Print($"New approval created:{approvalResult.Id} - Version {approvalResult.Version}");
        }

        private async Task DeleteApprovalVersion()
        {
            Printer.PrintStepTitle("Deletes An Existing Version Of An Existing Approval");

            Console.WriteLine("Deleting version...");
            var result = await _apiClient.Approvals.DeleteVersion(_testContainer.Approval.Id, _testContainer.Approval.Versions.LastOrDefault().Version);
            
            Printer.Print($"Successfully deleted ({result})...");
        }

        private async Task UpsertApprovalGroupUser()
        {
            Printer.PrintStepTitle("Upserts Existing User to Approval Group");

            var parameters = new ApprovalGroupUserParameters
            {
                AllowDecision = true,
                AllowDownload = true,
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                CommentsEnabled = true,
                Enabled = true,
                Id = _testContainer.Approval.Id,
                UserId = _testContainer.RandomUserId,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Adding user...");

            await _apiClient.Approvals.UpsertApprovalGroupUser(parameters);

            Printer.Print("Successfully added user to approval group");
        }

        private async Task DeleteApprovalGroupUser()
        {
            Printer.PrintStepTitle("Deletes Existing User From Approval Group");

            var parameters = new ApprovalGroupRemoveUserParameters
            {
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Id = _testContainer.Approval.Id,
                UserId = _testContainer.User.Id,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Removing user...");
            await _apiClient.Approvals.RemoveApprovalGroupUser(parameters);

            Printer.Print("Successfully removed user from approval group");
        }

        private async Task ResetApprovalGroupUserDecision()
        {
            Printer.PrintStepTitle("Resets Existing User Decision");

            var parameters = new ApprovalGroupUserParameters
            {
                Id = _testContainer.Approval.Id,
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                UserId = _testContainer.RandomUserId,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Resetting decisions...");
            await _apiClient.Approvals.ResetApprovalGroupUserDecision(parameters);

            Printer.Print("Successfully reset user decesion");
        }

        private async Task UpdateApprovalGroupUserDecision()
        {
            Printer.PrintStepTitle("Updates Existing User Decision");

            var parameters = new ApprovalGroupUserDecisionParameters
            {
                Id = _testContainer.Approval.Id,
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                UserId = _testContainer.RandomUserId,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version,
                Decision = Api.Data.ApproverDecision.Approved
            };

            Printer.Print("Updating decisions...");

            await _apiClient.Approvals.SetApprovalGroupUserDecision(parameters);
            Printer.Print("Successfully set user decesion");
        }

        private async Task CreateApprovalGroup()
        {
            Printer.PrintStepTitle("Creates New Approval Group");

            var parameters = new ApprovalGroupCreateParameters
            {
                ApprovalId = _testContainer.Approval.Id,
                Name = "Test Group",
                NumberOfDecisionsRequired = 3,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Creating group...");

            var _ = await _apiClient.Approvals.AddApprovalGroup(parameters);
            Printer.Print($"Successfully created approval group!");
        }

        private async Task UpdateApprovalGroup()
        {
            Printer.PrintStepTitle("Updating group...");

            var parameters = new ApprovalGroupUpdateParameters
            {
                ApprovalId = _testContainer.Approval.Id,
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Name = "Updated group",
                NumberOfDecisionsRequired = 5,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            
            var _ = await _apiClient.Approvals.UpdateApprovalGroup(parameters);
            Printer.Print($"Successfully updated approval group!");
        }

        private async Task ResetAllApprovalGroupDecisions()
        {
            Printer.PrintStepTitle("Resets All User Decisions Of Approval Group");
            
            var parameters = new ApprovalGroupUserParameters
            {
                Id = _testContainer.Approval.Id,
                ApprovalGroupId = _testContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version
            };

            await _apiClient.Approvals.ResetAllApprovalGroupUserDecisions(parameters);
            Printer.Print($"Successfully reset approval group decisions.");
        }

        private async Task CreatePersonalUrl()
        {
            Printer.PrintStepTitle("Creates Purl");

            var parameters = new PersonalUrlCreateParameters
            {
                ApprovalId = _testContainer.Approval.Id,
                Version = _testContainer.Approval.Versions.FirstOrDefault().Version,
                UserId = _testContainer.User.Id
            };

            Printer.Print("Creating PURL...");
            var purl = await _apiClient.Approvals.CreatePersonalUrl(parameters);

            Printer.Print($"Successfully created URL: {purl.Url}");
        }
    }
}
