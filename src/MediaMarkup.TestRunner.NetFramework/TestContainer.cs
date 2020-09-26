using MediaMarkup.Api.Models;
using System;

namespace MediaMarkup.TestRunner.NetFramework
{
    public class TestContainer
    {
        internal User User { private set; get; }

        internal Approval Approval { private set; get; }

        internal string RandomUserId { private set; get; }
        
        internal ApprovalDraft ApprovalDraft { private set; get; }

        internal TestContainer()
        {
            CreateTestUser();
            RandomUserId = Guid.NewGuid().ToString();
        }

        private void CreateTestUser()
        {
            User = new User
            {
                EmailAddress = "test@test.com",
                FirstName = "Test Firstname",
                LastName = "Test Lastname",
                Role = "User",
                WebLoginEnabled = true
            };
        }

        internal void UpdateUser(User updatedUser)
        {
            User.EmailAddress = updatedUser.EmailAddress;
            User.FirstName = updatedUser.FirstName;
            User.Id = updatedUser.Id;
            User.LastName = updatedUser.LastName;
            User.Role = updatedUser.Role;
            User.WebLoginEnabled = updatedUser.WebLoginEnabled;
        }

        internal void SetApproval(Approval approval)
        {
            Approval = approval;
        }

        internal void SetApprovalDraft(ApprovalDraft approvalDraft)
        {
            ApprovalDraft = approvalDraft;
        }
    }
}