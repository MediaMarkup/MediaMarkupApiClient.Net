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
                EmailAddress = "TestUserApiMethods@brightertools.com",
                UserRole = UserRole.Administrator,
                Password = "",
                WebLoginEnabled = false
            };
            var userCreated = await _context.ApiClient.Users.Create(userCreateParameters);

            Assert.True(userCreated != null);

            // Get the user by id
            var retrievedUserById = await _context.ApiClient.Users.GeById(userCreated.Id, true);
            
            Assert.True(retrievedUserById != null && userCreated.Id == retrievedUserById.Id);

            // Update the user
            var userUpdateParameters = new UserUpdateParameters
            {
                Id = retrievedUserById.Id,
                FirstName = "updated",
                LastName = "updated",
                EmailAddress = userCreated.EmailAddress,
                UserRole = UserRole.Administrator,
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
            var userExists = await _context.ApiClient.Users.GeById(userCreated.Id);
            
            Assert.True(userExists == null);
        }

        [Fact, TestPriority(4)]
        public async Task TestApprovalApiMethods()
        {
            var approvalId = string.Empty;
            var approvalOwnerUserid = string.Empty;
            var approvalReviewer1Id = string.Empty;
            var approvalReviewer2Id = string.Empty;
            var approvalReviewer3Id = string.Empty;
            var approvalReviewer4Id = string.Empty;

            // Create test user for approval owner
            var userCreateParameters = new UserCreateParameters
            {
                FirstName = "ApprovalOwner",
                LastName = $"Test {Guid.NewGuid().ToString()}",
                EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
            };
            var userCreated = await _context.ApiClient.Users.Create(userCreateParameters);

            Assert.True(userCreated != null);

            approvalOwnerUserid = userCreated.Id;

            // Create test user for approval reviewer added on approval creation
            userCreateParameters = new UserCreateParameters
            {
                FirstName = "ApprovalUser1",
                LastName = $"Test {Guid.NewGuid().ToString()}",
                EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
            };
            var userCreated2 = await _context.ApiClient.Users.Create(userCreateParameters);

            approvalReviewer1Id = userCreated2.Id;

            userCreateParameters = new UserCreateParameters
            {
                FirstName = "ApprovalUser2",
                LastName = $"Test {Guid.NewGuid().ToString()}",
                EmailAddress = $"Test {Guid.NewGuid().ToString("N")}@brightertools.com"
            };
            var userCreated3 = await _context.ApiClient.Users.Create(userCreateParameters);

            approvalReviewer2Id = userCreated3.Id;

            Assert.True(userCreated2 != null);

            try
            {
                var parameters = new ApprovalListRequestParameters();
                //parameters.OwnerIdFilter = "7b3529ffe49c4cc2ad6dae1cd03b371b";
                parameters.UserIdFilter = "7b3529ffe49c4cc2ad6dae1cd03b371b";
                var approvalListResult = await _context.ApiClient.Approvals.GetList(parameters);

                var approvalCount = approvalListResult.TotalCount;

                var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var testFile = Path.Combine(baseDir, "Files", "PDFTestFile2Pages.pdf");

                var approvalCreateParameters = new ApprovalCreateParameters
                {
                    Name = "TestApproval",
                    OwnerUserId = "7b3529ffe49c4cc2ad6dae1cd03b371b",
                    NumberOfDecisionsRequired = 0,
                    Deadline = DateTime.Now.AddDays(5),
                    Reviewers = new List<ApprovalGroupUser> { new ApprovalGroupUser{ UserId = approvalReviewer1Id, AllowDecision = true, AllowDownload = true, CommentsEnabled = true} }
                };

                // Upload Approval
                var createResult = await _context.ApiClient.Approvals.Create(testFile, approvalCreateParameters);

                Assert.True(!string.IsNullOrWhiteSpace(createResult.Id));

                approvalId = createResult.Id;

                // we update the approval name and set a deadline
                var approvalUpdateParameters = new ApprovalUpdateParameters
                {
                    Id = approvalId,
                    Name = "TestApproval Updated",
                    Active = true,
                    OwnerUserId = "7b3529ffe49c4cc2ad6dae1cd03b371b"
                };

                await _context.ApiClient.Approvals.Update(approvalUpdateParameters);

                await _context.ApiClient.Approvals.UpdateName(new ApprovalUpdateNameParameters { Id = approvalId, Name = "TestApproval Updated 3"});

                await _context.ApiClient.Approvals.UpdateOwnerUserId(new ApprovalUpdateOwnerUserIdParameters { Id = approvalId, OwnerUserId = approvalOwnerUserid});

                await _context.ApiClient.Approvals.SetActive(new ApprovalSetActiveParameters { Id = approvalId, Active = false});

                await _context.ApiClient.Approvals.SetActive(new ApprovalSetActiveParameters { Id = approvalId, Active = true});

                var approvalCreateVersionParameters = new ApprovalCreateVersionParameters
                {
                    ApprovalId = approvalId,
                    CopyApprovalGroups = true,
                    LockPreviousVersion = false
                };

                var newVersionResult = await _context.ApiClient.Approvals.CreateVersion(testFile, approvalCreateVersionParameters);

                // Create a new approval group




                // We add 2 reviewers to the approval

                // We remove 1 reviewer from the approval

                // We add a new approval version

                // We add a new reviewer to the new version

                // we add a review group to the new version

                // we add a new user to the new group

                // we add a third group

                // we delete the third group

                // we add a new version

                // we lock the new version

                // we delete version 1

                // we get the approval details
            }
            finally
            {
                // delete approval
                await _context.ApiClient.Approvals.Delete(approvalId);

                // delete users
                await _context.ApiClient.Users.Delete(approvalOwnerUserid);
                await _context.ApiClient.Users.Delete(approvalReviewer1Id);
                await _context.ApiClient.Users.Delete(approvalReviewer2Id);
                await _context.ApiClient.Users.Delete(approvalReviewer3Id);
                await _context.ApiClient.Users.Delete(approvalReviewer4Id);
            }

            
        }

    }
}