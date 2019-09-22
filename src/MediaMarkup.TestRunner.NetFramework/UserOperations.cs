using MediaMarkup.Api.Models;
using System;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class UserOperations
    {
        private static ApiClient _apiClient;

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

            var parameters = new UserCreateParameters
            {
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                Role = role
            };

            var invitation = await _apiClient.Users.Create(parameters);
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
    }
}
