using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MediaMarkup.Api.Models;
using MediaMarkup.Tests.TestOrdering;
using Xunit;

namespace MediaMarkup.Tests
{
    [TestCaseOrderer("MediaMarkup.Tests.TestOrdering.PriorityOrderer", "MediaMarkup.Tests")]
    public class MediaMarkupApiTests : IClassFixture<TestContextFixture>
    {
        private readonly TestContextFixture _context;

        public MediaMarkupApiTests(TestContextFixture fixture)
        {
            _context = fixture;
        }

        [Fact, TestPriority(1)]
        public async Task GetAccessToken()
        {
            _context.ApiClient = new ApiClient(_context.Settings);

            _context.AccessToken = await _context.ApiClient.InitializeAsync();

            Assert.True(!string.IsNullOrWhiteSpace(_context.AccessToken));
        }

        [Fact, TestPriority(2)]
        public async Task CheckAuthentication()
        {
            var result = await _context.ApiClient.Authenticated();

            Assert.True(result);
        }

        [Fact, TestPriority(3)]
        public async Task TestUserApiMethods()
        {
            // Create test user
            var userCreateParameters = new UserCreateParameters
            {
                FirstName = "TestUserApiMethods",
                LastName = "TestUserApiMethods",
                EmailAddress = $"TestUserApiMethods{Guid.NewGuid():N}@brightertools.com",
                Role = UserRole.Administrator,
                Password = "",
                WebLoginEnabled = false
            };
            var userCreated = await _context.ApiClient.Users.Create(userCreateParameters);

            Assert.True(userCreated != null);

            // Get the user by id
            var retrievedUserById = await _context.ApiClient.Users.GetById(userCreated.Id, true);

            Assert.True(retrievedUserById != null && userCreated.Id == retrievedUserById.Id);

            // Update the user
            var userUpdateParameters = new UserUpdateParameters
            {
                Id = retrievedUserById.Id,
                FirstName = "updated",
                LastName = "updated",
                EmailAddress = userCreated.EmailAddress,
                Role = UserRole.Administrator,
                WebLoginEnabled = true
            };
            var updatedUser = await _context.ApiClient.Users.Update(userUpdateParameters);

            // Get the user by email
            var retrievedUserByEmail = await _context.ApiClient.Users.GetByEmail(userCreated.EmailAddress);
            Assert.True(retrievedUserById != null && userCreated.Id == retrievedUserByEmail.Id);

            // Check the user is updated
            Assert.True(retrievedUserByEmail.FirstName == "updated");

            // Delete the user
            await _context.ApiClient.Users.Delete(userCreated.Id);

            // Get the user by if to see if user exists
            var userExists = await _context.ApiClient.Users.GetById(userCreated.Id);

            Assert.True(userExists == null);


        }

        //[Fact, TestPriority(4)]
        //public async Task TestApprovalNotesExport()
        //{
    //        try
    //        {
    //            var tempFilename = @"c:\temp\testreport.pdf";

    //            if (File.Exists(tempFilename))
    //            {
    //                File.Delete(tempFilename);
    //            }

    //var fileContent = await _context.ApiClient.Approvals.ExportAnnotationReport(new ExportReportParameters
    //{
    //    Id = "",
    //    Version = 0,
    //    ApprovalGroupId = ""
    //});

    //File.WriteAllBytes(tempFilename, fileContent);

    //            Assert.True(File.Exists(tempFilename));
    //        }
    //        catch (Exception exception)
    //        {

    //        }
        //}

        // Todo: Full Approval Testing via API
        //[Fact, TestPriority(6)]
        //public async Task TestApprovalApiMethods()
        //{
        //    var testAdminOwnerUserId = _context.TestSettings.AdminUserId;

        //    Assert.True(!string.IsNullOrWhiteSpace(testAdminOwnerUserId));

        //    var approvalId = string.Empty;
        //    var approvalOwnerUserid = string.Empty;
        //    var approvalReviewer1Id = string.Empty;
        //    var approvalReviewer2Id = string.Empty;
        //    var approvalReviewer3Id = string.Empty;
        //    var approvalReviewer4Id = string.Empty;

        //    // Create test user for approval owner
        //    var userCreateParameters = new UserCreateParameters
        //    {
        //        FirstName = "ApprovalOwner",
        //        LastName = $"Test {Guid.NewGuid().ToString()}",
        //        EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
        //    };
        //    var userCreated = await _context.ApiClient.Users.Create(userCreateParameters);

        //    Assert.True(userCreated != null);

        //    approvalOwnerUserid = userCreated.Id;

        //    // Create test user for approval reviewer added on approval creation
        //    userCreateParameters = new UserCreateParameters
        //    {
        //        FirstName = "ApprovalUser1",
        //        LastName = $"2Test{Guid.NewGuid().ToString()}",
        //        EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
        //    };
        //    var userCreated2 = await _context.ApiClient.Users.Create(userCreateParameters);

        //    approvalReviewer1Id = userCreated2.Id;

        //    userCreateParameters = new UserCreateParameters
        //    {
        //        FirstName = "ApprovalUser2",
        //        LastName = $"T2est{Guid.NewGuid().ToString()}",
        //        EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
        //    };
        //    var userCreated3 = await _context.ApiClient.Users.Create(userCreateParameters);

        //    approvalReviewer2Id = userCreated3.Id;

        //    userCreateParameters = new UserCreateParameters
        //    {
        //        FirstName = "ApprovalUser3",
        //        LastName = $"3Test{Guid.NewGuid().ToString()}",
        //        EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
        //    };
        //    var userCreated4 = await _context.ApiClient.Users.Create(userCreateParameters);

        //    approvalReviewer3Id = userCreated4.Id;

        //    userCreateParameters = new UserCreateParameters
        //    {
        //        FirstName = "ApprovalUser4",
        //        LastName = $"4Test{Guid.NewGuid().ToString()}",
        //        EmailAddress = $"Test {Guid.NewGuid():N}@brightertools.com"
        //    };
        //    var userCreated5 = await _context.ApiClient.Users.Create(userCreateParameters);

        //    approvalReviewer4Id = userCreated5.Id;

        //    Assert.True(userCreated2 != null);

        //    try
        //    {
        //        var parameters = new ApprovalListRequestParameters {UserIdFilter = testAdminOwnerUserId};
        //        //parameters.OwnerIdFilter = testAdminOwnerUserId;
        //        var approvalListResult = await _context.ApiClient.Approvals.GetList(parameters);

        //        var approvalCount = approvalListResult.TotalCount;

        //        var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //        var testFile = Path.Combine(baseDir, "Files", "PDFTestFile2Pages.pdf");

        //        var approvalCreateParameters = new ApprovalCreateParameters
        //        {
        //            Name = "TestApproval",
        //            OwnerUserId = testAdminOwnerUserId,
        //            NumberOfDecisionsRequired = 0,
        //            Deadline = DateTime.Now.AddDays(5),
        //            AddOwnerToInitialApprovalGroup = true,
        //            Reviewers = new List<ApprovalGroupUser>
        //            {
        //                new ApprovalGroupUser {UserId = approvalReviewer1Id, AllowDecision = true, AllowDownload = true, CommentsEnabled = true},
        //                new ApprovalGroupUser {UserId = approvalReviewer2Id, AllowDecision = true, AllowDownload = true, CommentsEnabled = true}
        //            }
        //        };

        //        // Upload Approval
        //        var createResult = await _context.ApiClient.Approvals.Create(testFile, approvalCreateParameters);

        //        Assert.True(!string.IsNullOrWhiteSpace(createResult.Id));

        //        approvalId = createResult.Id;

        //        // we update the approval name and set a deadline
        //        var approvalUpdateParameters = new ApprovalUpdateParameters
        //        {
        //            Id = approvalId,
        //            Name = "TestApproval Updated",
        //            Active = true,
        //            OwnerUserId = testAdminOwnerUserId
        //        };

        //        await _context.ApiClient.Approvals.Update(approvalUpdateParameters);

        //        await _context.ApiClient.Approvals.UpdateName(new ApprovalUpdateNameParameters {Id = approvalId, Name = "TestApproval Updated 3"});

        //        await _context.ApiClient.Approvals.UpdateOwnerUserId(new ApprovalUpdateOwnerUserIdParameters {Id = approvalId, OwnerUserId = approvalOwnerUserid});

        //        await _context.ApiClient.Approvals.SetActive(new ApprovalSetActiveParameters {Id = approvalId, Active = false});

        //        await _context.ApiClient.Approvals.SetActive(new ApprovalSetActiveParameters {Id = approvalId, Active = true});

        //        var approvalCreateVersionParameters = new ApprovalCreateVersionParameters
        //        {
        //            ApprovalId = approvalId,
        //            CopyApprovalGroups = true,
        //            LockPreviousVersion = false
        //        };

        //        // Create a new Version (copy all groups / users)
        //        var newVersionWithExistingGroupsResult = await _context.ApiClient.Approvals.CreateVersion(testFile, approvalCreateVersionParameters);

        //        approvalCreateVersionParameters = new ApprovalCreateVersionParameters
        //        {
        //            ApprovalId = approvalId,
        //            CopyApprovalGroups = false,
        //            LockPreviousVersion = true,
        //            AddOwnerToInitialApprovalGroup = true,
        //            Reviewers = new List<ApprovalGroupUser>
        //            {
        //                new ApprovalGroupUser {UserId = approvalReviewer2Id, AllowDecision = true, AllowDownload = true, CommentsEnabled = true},
        //                new ApprovalGroupUser {UserId = approvalReviewer3Id, AllowDecision = true, AllowDownload = true, CommentsEnabled = true}
        //            }
        //        };

        //        var newVersionWithNewReviewersResult = await _context.ApiClient.Approvals.CreateVersion(testFile, approvalCreateVersionParameters);

        //        // Create a Personal Url for the vesion we just created
        //        var createOwnerPersonalUrlResponse = await _context.ApiClient.Approvals.CreatePersonalUrl(new PersonalUrlCreateParameters
        //        {
        //            UserId = testAdminOwnerUserId,
        //            Version = 3,
        //            ApprovalId = approvalId
        //        });

        //        // Note: To test the url, debug the tests and set a breakpoint on the line below, get the url and try it in a browser.
        //        // The approval will be deleted below, so test the url manually then contine..
        //        var url = createOwnerPersonalUrlResponse.Url;

        //        // Todo: Implement the rest of the tests / api calls

        //        await _context.ApiClient.Approvals.RemoveApprovalGroupUser(new ApprovalGroupRemoveUserParameters
        //        {
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            UserId = approvalReviewer2Id
        //            //ApprovalGroupId not set..should pick up only group
        //        });

        //        await _context.ApiClient.Approvals.RemoveApprovalGroupUser(new ApprovalGroupRemoveUserParameters
        //        {
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            UserId = approvalReviewer3Id
        //            //ApprovalGroupId not set..should pick up only group
        //        });

        //        await _context.ApiClient.Approvals.AddApprovalGroupUser(new ApprovalGroupUserParameters
        //        {
        //            UserId = approvalReviewer2Id,
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            AllowDecision = true,
        //            AllowDownload = true,
        //            AllowVersionSelection = true,
        //            //ApprovalGroupId not set to pick up first group
        //        });

        //        // Create a Personal Url for the vesion we just created
        //        var createPersonalUrlForUserIn2GroupsResponse = await _context.ApiClient.Approvals.CreatePersonalUrl(new PersonalUrlCreateParameters
        //        {
        //            UserId = approvalReviewer2Id,
        //            Version = 3,
        //            ApprovalId = approvalId
        //        });

        //        // Note: To test the url, debug the tests and set a breakpoint on the line below, get the url and try it in a browser.
        //        // The approval will be deleted below, so test the url manually then contine..
        //        var url2 = createPersonalUrlForUserIn2GroupsResponse.Url;

        //        // Create a Personal Url for the vesion we just created
        //        var createPersonalUrlForUserIn1GroupsResponse = await _context.ApiClient.Approvals.CreatePersonalUrl(new PersonalUrlCreateParameters
        //        {
        //            UserId = approvalReviewer2Id,
        //            Version = 3,
        //            ApprovalId = approvalId
        //        });

        //        // Note: To test the url, debug the tests and set a breakpoint on the line below, get the url and try it in a browser.
        //        // The approval will be deleted below, so test the url manually then contine..

        //        var url3 = createPersonalUrlForUserIn1GroupsResponse.Url;

        //        var createPersonalUrlForObserverResponse = await _context.ApiClient.Approvals.CreatePersonalUrl(new PersonalUrlCreateParameters
        //        {
        //            UserId = "",
        //            Observer = true,
        //            Version = 3,
        //            ApprovalId = approvalId
        //        });

        //        // Note: To test the url, debug the tests and set a breakpoint on the line below, get the url and try it in a browser.
        //        // The approval will be deleted below, so test the url manually then contine..
        //        // This is an observer URL (should see all groups, notes, comments and no decisions or adding comments)
        //        var url4 = createPersonalUrlForObserverResponse.Url;

        //        // Create a Personal Url for the vesion we just created
        //        var createPersonalUrlForUserInZeroGroupsResponse = await _context.ApiClient.Approvals.CreatePersonalUrl(new PersonalUrlCreateParameters
        //        {
        //            UserId = approvalReviewer4Id,
        //            Version = 3,
        //            ApprovalId = approvalId
        //        });

        //        // Note: To test the url, debug the tests and set a breakpoint on the line below, get the url and try it in a browser.
        //        // The approval will be deleted below, so test the url manually then contine..
        //        // This is an URL for an approver in zero groups
        //        var url5 = createPersonalUrlForUserInZeroGroupsResponse.Url;

        //        var urls = $"{url}{Environment.NewLine}{url2}{Environment.NewLine}{url3}{Environment.NewLine}{url4}{Environment.NewLine}{url4}{Environment.NewLine}";

        //        await _context.ApiClient.Approvals.AddApprovalGroupUser(new ApprovalGroupUserParameters
        //        {
        //            UserId = approvalReviewer3Id,
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            AllowDecision = true,
        //            AllowDownload = true,
        //            AllowVersionSelection = true,
        //            //ApprovalGroupId not set to pick up first group
        //        });

        //        await _context.ApiClient.Approvals.UpdateApprovalGroupUser(new ApprovalGroupUserParameters
        //        {
        //            UserId = approvalReviewer2Id,
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            AllowDecision = false,
        //            AllowDownload = false,
        //            AllowVersionSelection = false,
        //            //ApprovalGroupId not set to pick up first group
        //        });

        //        await _context.ApiClient.Approvals.UpdateApprovalGroupUser(new ApprovalGroupUserParameters
        //        {
        //            UserId = approvalReviewer3Id,
        //            Id = newVersionWithNewReviewersResult.Id,
        //            Version = newVersionWithNewReviewersResult.Version,
        //            AllowDecision = false,
        //            AllowDownload = false,
        //            AllowVersionSelection = false,
        //            //ApprovalGroupId not set to pick up first group
        //        });

        //        // Get Approval Details
        //        var approval = await _context.ApiClient.Approvals.Get(approvalId);

        //        var apprId = approval.Id;

        //        // We add 2 reviewers to the approval

        //        // We remove 1 reviewer from the approval

        //        // We add a new approval version

        //        // We add a new reviewer to the new version

        //        // we add a review group to the new version

        //        // we add a new user to the new group

        //        // we add a third group

        //        // we delete the third group

        //        // we add a new version

        //        // we lock the new version

        //        // we delete version 1

        //        // we get the approval details
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }
        //    finally
        //    {
        //        // delete approval
        //        await _context.ApiClient.Approvals.Delete(approvalId);

        //        // delete test users
        //        await _context.ApiClient.Users.Delete(approvalOwnerUserid);
        //        await _context.ApiClient.Users.Delete(approvalReviewer1Id);
        //        await _context.ApiClient.Users.Delete(approvalReviewer2Id);
        //        await _context.ApiClient.Users.Delete(approvalReviewer3Id);
        //        await _context.ApiClient.Users.Delete(approvalReviewer4Id);

        //        Assert.True(true);
        //    }
        //}
    }
}
