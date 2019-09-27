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

        async static Task Main(string[] args)
        {
            Console.WriteLine("This is a sample console app to drive MediaMarkup API");

            Console.WriteLine("--------");
            //Console.Write("Enter your Client ID:");
            string clientId = "C04F6E4281E34F9DB3FB14093FE3F8DE"; //Console.ReadLine();

            //Console.Write("Enter your Client Secret:");
            string clientScret = "03347DAA-A6AE-4F42-87C5-20D5A779368A"; //Console.ReadLine();

            var settings = new Settings
            {
                ApiBaseUrl = "https://localhost:44318",
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

                var programOperations = new Dictionary<int, Func<Task>>
                {
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
                    { 15, ApprovalOperations.DeleteApprovalGroupUser }
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
                //GetApprovals();
                //UpdateApproval();
                //ExportReport();


                //CreateApprovalVersion();
                //DeleteApprovalVersion();

                //CreateApprovalGroup();
                //UpdateApprovalGroup();

                //UpsertApprovalGroupUser();
                //UpdateApprovalGroupUserDecision();
                //ResetApprovalGroupUserDecision();
                //DeleteApprovalGroupUser();

                //ResetApprovalGroupDecision();

                //DeleteApproval();
                //DeleteUser();  

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
