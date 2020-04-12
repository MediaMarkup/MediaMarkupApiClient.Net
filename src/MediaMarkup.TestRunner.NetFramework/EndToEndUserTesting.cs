using MediaMarkup.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public static class EndToEndUserTesting
    {
        public static ApiClient ApiClient;
        public static TestContainer TestContainer;

        public static async Task CreateUser()
        {
            Printer.PrintStepTitle("Creating new user...");

            var payload = new UserCreateParameters
            {
                EmailAddress = TestContainer.User.EmailAddress,
                FirstName = TestContainer.User.FirstName,
                LastName = TestContainer.User.LastName,
                Password = Guid.NewGuid().ToString(),
                Role = TestContainer.User.Role,
                WebLoginEnabled = TestContainer.User.WebLoginEnabled
            };

            var userCreated = await ApiClient.Users.Create(payload);
            Printer.Print($"New user created!");

            TestContainer.UpdateUser(userCreated);
        }

        public static async Task GetUserById()
        {
            Printer.PrintStepTitle($"Getting user details by id {TestContainer.User.Id}...");

            var _ = await ApiClient.Users.GetById(TestContainer.User.Id, true);
            Printer.Print($"User found!");
        }

        public static async Task UpdateUser()
        {
            Printer.PrintStepTitle("Updating user...");

            var payload = new UserUpdateParameters
            {
                FirstName = "Updated Firstname",
                LastName = "Updated Lastname",
                WebLoginEnabled = false,
                Password = Guid.NewGuid().ToString()
            };

            var updatedUser = await ApiClient.Users.Update(TestContainer.User.Id, payload);

            Assert.AreEqual(payload.FirstName, updatedUser.FirstName);
            Assert.AreEqual(payload.LastName, updatedUser.LastName);
            Assert.AreEqual(payload.WebLoginEnabled, updatedUser.WebLoginEnabled);
            Printer.Print("User updated!");

            TestContainer.UpdateUser(updatedUser);
        }

        public static async Task GetUserByEmail()
        {
            Printer.PrintStepTitle("Getting user by email...");

            var _ = await ApiClient.Users.GetByEmail(TestContainer.User.EmailAddress, true);
        }
    }
}
