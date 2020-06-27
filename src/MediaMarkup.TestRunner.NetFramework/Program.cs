using MediaMarkup.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    class Program
    {
        static UserOperations UserOperations;
        static ApprovalOperations ApprovalOperations;
        static EndToEndTesting EndToEndTesting;

        async static Task Main(string[] args)
        {
            Console.WriteLine("This is a sample console app to drive MediaMarkup API");

            Console.WriteLine("--------");
            Console.Write("Enter your Client ID:");
            string clientId = Console.ReadLine();

            Console.Write("Enter your Client Secret:");
            string clientScret = Console.ReadLine();

            var settings = new Settings
            {
                ApiBaseUrl = "https://api.mediamarkup.com/",
                ClientId = clientId,
                SecretKey = clientScret
            };



            try
            {
                Console.WriteLine("Authorizing client details and retrieving AccessToken...");
                var apiClient = new ApiClient(settings);
                var token = await apiClient.InitializeAsync();
                Console.WriteLine("AccessToken acquired, ready to drive!");
                Console.WriteLine($"\n{token}\n");

                Console.WriteLine("Successfully authorized the connection, press any key to continue");
                Console.ReadKey();

                UserOperations = new UserOperations(apiClient);
                ApprovalOperations = new ApprovalOperations(apiClient);
                EndToEndTesting = new EndToEndTesting(apiClient);

                var programOperations = new Dictionary<int, Func<Task>>
                {
                    { 0, UserOperations.CreateUser },
                    { 1, UserOperations.InviteUser },
                    { 2, UserOperations.GetUserByEmail },
                    { 3, UserOperations.GetUserById },
                    { 4, UserOperations.UpdateUser },
                    { 5, UserOperations.DeleteUserById },

                    { 6, ApprovalOperations.CreateApproval },
                    { 7, ApprovalOperations.GetApproval },
                    { 8, ApprovalOperations.UpdateApproval },
                    { 9, ApprovalOperations.DeleteApproval },
                    { 10, ApprovalOperations.ExportApprovalReport },
                    { 11, ApprovalOperations.GetApprovalList },
                    { 12, ApprovalOperations.CreateApprovalVersion },
                    { 13, ApprovalOperations.DeleteApprovalVersion },
                    { 14, ApprovalOperations.UpsertApprovalGroupUser },
                    { 15, ApprovalOperations.DeleteApprovalGroupUser },
                    { 16, ApprovalOperations.ResetApprovalGroupUserDecision },
                    { 17, ApprovalOperations.UpdateApprovalGroupUserDecision },
                    { 18, ApprovalOperations.CreateApprovalGroup },
                    { 19, ApprovalOperations.UpdateApprovalGroup },
                    { 20, ApprovalOperations.CreateApprovalGroups },
                    { 21, ApprovalOperations.ResetAllApprovalGroupDecisions },
                    { 22, ApprovalOperations.CreatePersonalUrl },
                    { 99, EndToEndTesting.RunEndToEndTest }
                };



                int operationSelected = -1;

                do
                {
                    Console.Clear();
                    Console.WriteLine("\nChoose an action to run on Public API. (-1 to exit)\n");
                    foreach (var programOperation in programOperations)
                    {
                        Console.WriteLine($"{programOperation.Key}. {programOperation.Value.Method.Name}");
                    }

                    Console.Write("Run: ");
                    var validSelection = int.TryParse(Console.ReadLine(), out operationSelected);

                    if (!validSelection)
                    {
                        Console.WriteLine("Couldn't recognize the input. Please enter a number to choose from the menu");
                        continue;
                    }

                    if (operationSelected == -1) break;

                    try
                    {
                        var operation = programOperations[operationSelected];

                        Console.Clear();
                        await operation();
                        Console.WriteLine("\n.....press any key to continue...");
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An exception occured, press any key to continue...");
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                    }
                }
                while (operationSelected != -1);

                Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("End of drive...Press any key to close the program.");
            }
            catch (ApiException e)
            {

                Console.WriteLine("An error occured...");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine(e.Message);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!");
            }
        }
    }
}
