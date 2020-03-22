using MediaMarkup.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public class EndToEndTesting
    {
        private readonly ApiClient _apiClient;

        private TestContainer _testContainer;

        internal EndToEndTesting(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _testContainer = new TestContainer();
        }

        private void Print(string message)
        {
            Console.WriteLine(message);
        }

        private void Print(User user)
        {
            Console.WriteLine("\n-----------\n");
            Console.WriteLine($"{user.EmailAddress}");
            Console.WriteLine($"{user.Id}");
            Console.WriteLine($"{user.FirstName} {user.LastName} ({user.Role})");
            Console.WriteLine($"Web Login: {user.WebLoginEnabled}");
            Console.WriteLine($"Is Owner: {user.AccountOwner}");
            Console.WriteLine("\n-----------\n");
        }

        private async Task Cleanup()
        {
            Print("\n-------------------\n");
            Print("Cleaning up......");

            Print("\n-------------------\n");
            Print("Deleting user...");
            await _apiClient.Users.Delete(_testContainer.User.Id, true);

            Print("End of test....");
        }

        public async Task RunEndToEndTest()
        {
            await CreateUser();
            Print(_testContainer.User);

            await GetUserById();
            Print(_testContainer.User);

            await UpdateUser();
            Print(_testContainer.User);

            await GetUserByEmail();
            Print(_testContainer.User);

            await Cleanup();
        }

        private async Task CreateUser()
        {
           Print("Creating new user...");

            var payload = new UserCreateParameters
            {
                EmailAddress = _testContainer.User.EmailAddress,
                FirstName = _testContainer.User.FirstName,
                LastName = _testContainer.User.LastName,
                Password = Guid.NewGuid().ToString(),
                Role = _testContainer.User.Role,
                WebLoginEnabled = _testContainer.User.WebLoginEnabled
            };

            var userCreated = await _apiClient.Users.Create(payload);
            Print($"New user created!");

            _testContainer.UpdateUser(userCreated);
        }

        private async Task GetUserById()
        {
            Print($"Getting user details by id {_testContainer.User.Id}...");
            var existingUser = await _apiClient.Users.GetById(_testContainer.User.Id, true);
            Print($"User found!");
        }

        private async Task UpdateUser()
        {
            Print("Updating user...");
            var payload = new UserUpdateParameters
            {
                FirstName = "Updated Firstname",
                LastName = "Updated Lastname",
                WebLoginEnabled = false,
                Password = Guid.NewGuid().ToString()
            };

            var updatedUser = await _apiClient.Users.Update(_testContainer.User.Id, payload);
            
            Assert.AreEqual(payload.FirstName, updatedUser.FirstName);
            Assert.AreEqual(payload.LastName, updatedUser.LastName);
            Assert.AreEqual(payload.WebLoginEnabled, updatedUser.WebLoginEnabled);

            Print("User updated!");
            _testContainer.UpdateUser(updatedUser);
        }

        private async Task GetUserByEmail()
        {
            Print("Getting user by email...");
            var _ = await _apiClient.Users.GetByEmail(_testContainer.User.EmailAddress, true);
        }
    }
}
