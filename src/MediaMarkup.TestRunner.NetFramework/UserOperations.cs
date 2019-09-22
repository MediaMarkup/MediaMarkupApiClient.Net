using MediaMarkup.Api.Models;
using System;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public class UserOperations
    {
        private static ApiClient _apiClient;

        public UserOperations(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task InviteUser()
        {
            Console.Write("Enter an email address to invite:");
            string email = Console.ReadLine();

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

        public async Task GetUserByEmail()
        {
            Console.Write("Enter an email address to search:");
            string email = Console.ReadLine();

            Console.WriteLine($"Searching {email}...");
            var user = await _apiClient.Users.GetByEmail(email);

            Console.WriteLine($"{user.FirstName} {user.LastName} found.");
        }
    }
}
