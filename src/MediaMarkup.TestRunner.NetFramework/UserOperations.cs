using MediaMarkup.Api.Models;
using System;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class UserOperations
    {
        private readonly ApiClient _apiClient;

        internal UserOperations(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        internal async Task InviteUser()
        {
            Printer.PrintStepTitle("User Invitation");
            Console.Write("Enter an email address to invite:");
            string email = Console.ReadLine();

            if (email == "-1") return;

            Console.Write("Enter first name:");
            string firstName = Console.ReadLine();

            Console.Write("Enter last name:");
            string lastName = Console.ReadLine();

            Console.Write($"Enter role ({string.Join(",", UserRole.Roles)}):");
            string role = Console.ReadLine();

            var parameters = new UserInviteParameters
            {
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                Role = role
            };

            var invitation = await _apiClient.Users.Invite(parameters);
            Console.WriteLine($"{firstName} {lastName} has been invited. Following URL is sent in the invitation e-mail");
            Console.WriteLine(invitation.Url);
        }

        internal async Task GetUserById()
        {
            Printer.PrintStepTitle("Get User Details By ID");
            Console.Write("Enter a User ID to search:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            Console.WriteLine($"Searching {id}...");
            var user = await _apiClient.Users.GetById(id);

            Printer.PrintUser(user);
        }

        internal async Task UpdateUser()
        {
            Printer.PrintStepTitle("Update User Details");

            Console.Write("Enter a User ID:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            var existingUser = await _apiClient.Users.GetById(id);
            Printer.PrintUser(existingUser);
            if (existingUser == null) return;

            Console.Write("Enter New Name:");
            string newName = Console.ReadLine();

            Console.Write("Enter New Lastname:");
            string newLastName = Console.ReadLine();

            Console.Write("Enter New Password (min 6 characters):");
            string newPassword = Console.ReadLine();

            var parameters = new UserUpdateParameters
            {
                FirstName = newName,
                LastName = newLastName,
                Password = newPassword
            };

            var updatedUser = await _apiClient.Users.Update(id, parameters);
            Printer.PrintUser(updatedUser);
        }

        internal async Task GetUserByEmail()
        {
            Printer.PrintStepTitle("Get User Details By Email");

            Console.Write("Enter an email address to search:");
            string email = Console.ReadLine();

            if (email == "-1") return;

            Console.WriteLine($"Searching {email}...");
            var user = await _apiClient.Users.GetByEmail(email);

            Printer.PrintUser(user);
        }

        internal async Task DeleteUserById()
        {
            Printer.PrintStepTitle("Delete User By ID");

            Console.Write("Enter User ID:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            Console.WriteLine($"Deleting {id}...");
            await _apiClient.Users.Delete(id);

            Console.WriteLine($"Searching {id}...");
            var user = await _apiClient.Users.GetById(id);
            Printer.PrintUser(user);

            Console.WriteLine($"Deleted {id} successfully...");
        }
    }
}
