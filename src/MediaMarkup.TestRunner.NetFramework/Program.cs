using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your Client ID:");
            string clientId = Console.ReadLine();

            Console.Write("Enter your Client Secret:");
            string clientScret = Console.ReadLine();

            var settings = new Settings
            {
                ApiBaseUrl = "https://localhost:44318",
                ClientId = clientId,
                SecretKey = clientScret
            };

            try
            {
                var apiClient = new ApiClient(settings);
                apiClient.InitializeAsync().Wait();

                SearchUserByEmail(apiClient);
            }
            catch (ApiException apie )
            {

                Console.WriteLine("An error occured...");
                Console.WriteLine(apie.Message);
            }
        }

        private static void SearchUserByEmail(ApiClient apiClient)
        {
            Console.Write("Enter an email address to search:");
            string email = Console.ReadLine();

            Console.WriteLine($"Searching {email}...");
            var user = apiClient.Users.GetByEmail(email).Result;

            Console.WriteLine($"{user.FirstName} {user.LastName} found.");
        }
    }
}
