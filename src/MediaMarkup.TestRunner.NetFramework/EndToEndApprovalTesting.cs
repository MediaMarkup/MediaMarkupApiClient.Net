using MediaMarkup.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public static class EndToEndApprovalTesting
    {
        public static ApiClient ApiClient;
        public static TestContainer TestContainer;

        public static async Task SetApprovalGroupReadOnly()
        {
            Printer.PrintStepTitle("Setting Approval Group Readonly");

            var parameters = new ApprovalGroupSetReadOnlyParameters
            {
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Id = TestContainer.Approval.Id,
                Readonly = true,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            await ApiClient.Approvals.SetApprovalGroupReadonly(parameters);
        }

        public static async Task SetApprovalVersionLock()
        {
            Printer.PrintStepTitle("Locking Approval");

            var parameters = new ApprovalVersionLockParameters
            {
                Id = TestContainer.Approval.Id,
                Locked = true,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Locking....");
            await ApiClient.Approvals.SetApprovalVersionLock(parameters);
        }

        public static async Task SetApprovalVersionUnLock()
        {
            Printer.PrintStepTitle("Unlocking Approval");

            var parameters = new ApprovalVersionLockParameters
            {
                Id = TestContainer.Approval.Id,
                Locked = false,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Locking....");
            await ApiClient.Approvals.SetApprovalVersionLock(parameters);
        }

        public static async Task CreateApproval()
        {
            Printer.PrintStepTitle("Uploads A Dummpy PDF For Approval");

            var random = new Random();
            var filePath = Path.Combine("dummy.pdf");

            var requestParameters = new ApprovalCreateParameters
            {
                Name = "Test Approval",
                NumberOfDecisionsRequired = random.Next(1, 10),
                OwnerUserId = TestContainer.User.Id,
            };

            Printer.Print("Creating new approval...");

            var approvalResult = await ApiClient.Approvals.Create(filePath, requestParameters);
            Printer.Print($"New approval created:{approvalResult.Id}");

            var approval = new Approval 
            {
                Id = approvalResult.Id
            };

            TestContainer.SetApproval(approval);
        }

        public static async Task GetApproval()
        {
            Printer.PrintStepTitle("Get Approval Details");

            var approval = await ApiClient.Approvals.Get(TestContainer.Approval.Id);
            Printer.Print("Approval found!");

            TestContainer.SetApproval(approval);
        }

        public static async Task UpdateApproval()
        {
            Printer.PrintStepTitle("Update Existing Approval");

            var parameters = new ApprovalUpdateParameters
            {
                Active = true,
                Name = "Updated Test Approval",
                OwnerUserId = TestContainer.User.Id
            };

            Printer.Print($"Updating approval...");

            var approval = await ApiClient.Approvals.Update(TestContainer.Approval.Id, parameters);
            Printer.Print("Approval updated!");

            TestContainer.SetApproval(approval);
        }

        public static async Task ExportApprovalReport()
        {
            Printer.PrintStepTitle("Export Report Of An Existing Approval");

            var parameters = new ExportReportParameters
            {
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Id = TestContainer.Approval.Id,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Exporting report...");
            var result = await ApiClient.Approvals.ExportAnnotationReport(parameters);

            Printer.Print($"Saving export file ({result.Length})...");

            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var reportPath = Path.Combine(directory, $"{Guid.NewGuid().ToString().Substring(0, 5)}.pdf");

            File.WriteAllBytes(reportPath, result);
            Printer.Print($"Saved to ${reportPath}");
        }

        public static async Task GetApprovalList()
        {
            Printer.PrintStepTitle("List and Filter Approvals");

            Printer.Print("Getting approvals...");

            var parameters = new ApprovalListRequestParameters
            {
                Page = 1,
                SortBy = SortBy.Status,
                SortDirection = SortDirection.Desc,
                TextFilter = TestContainer.Approval.Name,
                UserIdFilter = TestContainer.User.Id,
                Status = Status.All
            };
            var approvalListResult = await ApiClient.Approvals.GetList(parameters);
            approvalListResult.Approvals.ForEach(approval => Printer.PrintApproval(approval));
        }

        public static async Task CreateApprovalVersion()
        {
            Printer.PrintStepTitle("Creates A New Version Of An Existing Approval");

            var filePath = Path.Combine("dummy.pdf");

            var parameters = new ApprovalCreateVersionParameters
            {
                ApprovalId = TestContainer.Approval.Id,
                NumberOfDecisionsRequired = 3,
            };

            Console.WriteLine("Creating new approval...");

            var approvalResult = await ApiClient.Approvals.CreateVersion(filePath, parameters);
            Printer.Print($"New approval created:{approvalResult.Id} - Version {approvalResult.Version}");
        }

        public static async Task DeleteApprovalVersion()
        {
            Printer.PrintStepTitle("Deletes An Existing Version Of An Existing Approval");

            Console.WriteLine("Deleting version...");
            var result = await ApiClient.Approvals.DeleteVersion(TestContainer.Approval.Id, TestContainer.Approval.Versions.LastOrDefault().Version);
            
            Printer.Print($"Successfully deleted ({result})...");
        }

        public static async Task UpsertApprovalGroupUser()
        {
            Printer.PrintStepTitle("Upserts Existing User to Approval Group");

            var parameters = new ApprovalGroupUserParameters
            {
                AllowDecision = true,
                AllowDownload = true,
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                CommentsEnabled = true,
                Enabled = true,
                Id = TestContainer.Approval.Id,
                UserId = TestContainer.RandomUserId,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Adding user...");

            await ApiClient.Approvals.UpsertApprovalGroupUser(parameters);

            Printer.Print("Successfully added user to approval group");
        }

        public static async Task DeleteApprovalGroupUser()
        {
            Printer.PrintStepTitle("Deletes Existing User From Approval Group");

            var parameters = new ApprovalGroupRemoveUserParameters
            {
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Id = TestContainer.Approval.Id,
                UserId = TestContainer.RandomUserId,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Removing user...");
            await ApiClient.Approvals.RemoveApprovalGroupUser(parameters);

            Printer.Print("Successfully removed user from approval group");
        }

        public static async Task ResetApprovalGroupUserDecision()
        {
            Printer.PrintStepTitle("Resets Existing User Decision");

            var parameters = new ApprovalGroupUserParameters
            {
                Id = TestContainer.Approval.Id,
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                UserId = TestContainer.RandomUserId,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Resetting decisions...");
            await ApiClient.Approvals.ResetApprovalGroupUserDecision(parameters);

            Printer.Print("Successfully reset user decesion");
        }

        public static async Task UpdateApprovalGroupUserDecision()
        {
            Printer.PrintStepTitle("Updates Existing User Decision");

            var parameters = new ApprovalGroupUserDecisionParameters
            {
                Id = TestContainer.Approval.Id,
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                UserId = TestContainer.RandomUserId,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version,
                Decision = Api.Data.ApproverDecision.Approved
            };

            Printer.Print("Updating decisions...");

            await ApiClient.Approvals.SetApprovalGroupUserDecision(parameters);
            Printer.Print("Successfully set user decesion");
        }

        public static async Task CreateApprovalGroup()
        {
            Printer.PrintStepTitle("Creates New Approval Group");

            var parameters = new ApprovalGroupCreateParameters
            {
                ApprovalId = TestContainer.Approval.Id,
                Name = "Test Group",
                NumberOfDecisionsRequired = 3,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            Printer.Print("Creating group...");

            var approvalGroupCreateResult = await ApiClient.Approvals.AddApprovalGroup(parameters);
            var approvalGroup = new ApprovalGroup
            {
                Id = approvalGroupCreateResult.ApprovalGroupId,
                Name = parameters.Name,
                NumberOfDecisionsRequired = parameters.NumberOfDecisionsRequired.Value
            };
            
            TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.Add(approvalGroup);
            Printer.Print($"Successfully created approval group!");
        }

        public static async Task UpdateApprovalGroup()
        {
            Printer.PrintStepTitle("Updating group...");

            var parameters = new ApprovalGroupUpdateParameters
            {
                ApprovalId = TestContainer.Approval.Id,
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Name = "Updated group",
                NumberOfDecisionsRequired = 5,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            
            var _ = await ApiClient.Approvals.UpdateApprovalGroup(parameters);
            Printer.Print($"Successfully updated approval group!");
        }

        public static async Task CreateApprovalGroups()
        {
            Printer.PrintStepTitle("Creates New Approval Group");

            var parameters = new CreateApprovalGroupsParameters
            {
                ApprovalId = TestContainer.Approval.Id,
                ApprovalVersion = TestContainer.Approval.Versions.FirstOrDefault().Version,
                ApprovalGroups = new List<CreateApprovalGroupBatchItem>
                {
                    new CreateApprovalGroupBatchItem
                    {
                        Name = Guid.NewGuid().ToString(),
                        NumberOfDecisionsRequired = 1,
                        Users = new List<ApprovalGroupUser>
                        {
                            new ApprovalGroupUser
                            {
                                UserId = Guid.NewGuid().ToString()
                            },
                            new ApprovalGroupUser
                            {
                                UserId = Guid.NewGuid().ToString()
                            }
                        }
                    },
                    new CreateApprovalGroupBatchItem
                    {
                        Name = Guid.NewGuid().ToString(),
                        NumberOfDecisionsRequired = 1,
                        Users = new List<ApprovalGroupUser>
                        {
                            new ApprovalGroupUser
                            {
                                UserId = Guid.NewGuid().ToString()
                            },
                            new ApprovalGroupUser
                            {
                                UserId = Guid.NewGuid().ToString()
                            }
                        }
                    }
                }
            };
            
            Printer.Print("Creating groups...");

            var _ = await ApiClient.Approvals.CreateApprovalGroups(parameters);
            Printer.Print($"Successfully created approval groups!");
        }

        public static async Task ResetAllApprovalGroupDecisions()
        {
            Printer.PrintStepTitle("Resets All User Decisions Of Approval Group");
            
            var parameters = new ApprovalGroupUserParameters
            {
                Id = TestContainer.Approval.Id,
                ApprovalGroupId = TestContainer.Approval.Versions.FirstOrDefault().ApprovalGroups.FirstOrDefault().Id,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version
            };

            await ApiClient.Approvals.ResetAllApprovalGroupUserDecisions(parameters);
            Printer.Print($"Successfully reset approval group decisions.");
        }

        public static async Task CreatePersonalUrl()
        {
            Printer.PrintStepTitle("Creates Purl");

            var parameters = new PersonalUrlCreateParameters
            {
                ApprovalId = TestContainer.Approval.Id,
                Version = TestContainer.Approval.Versions.FirstOrDefault().Version,
                UserId = TestContainer.User.Id
            };

            Printer.Print("Creating PURL...");
            var purl = await ApiClient.Approvals.CreatePersonalUrl(parameters);

            Printer.Print($"Successfully created URL: {purl.Url}");
        }
    }
}
