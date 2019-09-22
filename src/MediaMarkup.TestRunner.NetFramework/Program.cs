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

        async static Task Main(string[] args)
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
                Console.WriteLine("Authorizing client details and retrieving AccessToken...");
                var apiClient = new ApiClient(settings);
                apiClient.InitializeAsync().Wait();

                UserOperations = new UserOperations(apiClient);

                await UserOperations.InviteUser();
                await UserOperations.GetUserByEmail();
                //GetUserById();
                //UpdateUser();
                
                
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
