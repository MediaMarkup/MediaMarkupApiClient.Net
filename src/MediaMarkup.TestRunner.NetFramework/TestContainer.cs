using MediaMarkup.Api.Models;
using System;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class TestContainer
    {
        internal User User { private set; get; }

        internal TestContainer()
        {
            CreateTestUser();
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
            User = updatedUser;
        }
    }
}