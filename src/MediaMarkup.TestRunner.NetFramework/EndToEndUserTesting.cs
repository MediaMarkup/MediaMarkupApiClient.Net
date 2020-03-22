using MediaMarkup.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class EndToEndUserTesting
    {
        private readonly ApiClient _apiClient;
        private readonly TestContainer _testContainer;

        internal EndToEndUserTesting(ApiClient apiClient, TestContainer testContainer)
        {
            _apiClient = apiClient;
            _testContainer = testContainer;
        }

        internal async Task Run()
        {
            await CreateUser();
            Printer.PrintUser(_testContainer.User);

            await GetUserById();
            Printer.PrintUser(_testContainer.User);

            await UpdateUser();
            Printer.PrintUser(_testContainer.User);

            await GetUserByEmail();
            Printer.PrintUser(_testContainer.User);
        }


        private async Task CreateUser()
        {
            Printer.PrintStepTitle("Creating new user...");

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
            Printer.Print($"New user created!");

            _testContainer.UpdateUser(userCreated);
        }

        private async Task GetUserById()
        {
            Printer.PrintStepTitle($"Getting user details by id {_testContainer.User.Id}...");

            var _ = await _apiClient.Users.GetById(_testContainer.User.Id, true);
            Printer.Print($"User found!");
        }

        private async Task UpdateUser()
        {
            Printer.PrintStepTitle("Updating user...");

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
            Printer.Print("User updated!");

            _testContainer.UpdateUser(updatedUser);
        }

        private async Task GetUserByEmail()
        {
            Printer.PrintStepTitle("Getting user by email...");

            var _ = await _apiClient.Users.GetByEmail(_testContainer.User.EmailAddress, true);
        }
    }
}
