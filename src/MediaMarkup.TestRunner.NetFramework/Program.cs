using MediaMarkup.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

                UserOperations = new UserOperations(apiClient);
                ApprovalOperations = new ApprovalOperations(apiClient);

                Console.WriteLine("To skip any of the steps, just enter -1 as the input");

                await UserOperations.InviteUser();
                await UserOperations.GetUserByEmail();

                await UserOperations.GetUserById();
                await UserOperations.UpdateUser();
                await UserOperations.DeleteUserById();

                await ApprovalOperations.CreateApproval();
                //GetUsersByEmails();
                //GetUsersByIds();
                //GetUsersByRoles();
                //GetUsersByQuery();

                //CreateApproval(); 
                //GetApproval();
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
                Console.ReadKey();
            }
            catch (ApiException e )
            {

                Console.WriteLine("An error occured...");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine(e.Message);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!");
            }
        }

        
    }
}
