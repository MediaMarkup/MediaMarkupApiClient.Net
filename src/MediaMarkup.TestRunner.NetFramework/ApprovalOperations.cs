using MediaMarkup.Api.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public class ApprovalOperations
    {
        private readonly ApiClient _apiClient;

        internal ApprovalOperations(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        internal async Task CreateApproval()
        {
            Printer.PrintStepTitle("Uploads A Dummpy PDF For Approval");
            Console.Write("Enter Name:");
            string name = Console.ReadLine();

            if (name == "-1") return;

            Console.Write("Enter Number of Decisions Required:");
            string numberOfDecisionsRequiredInput = Console.ReadLine();
            int.TryParse(numberOfDecisionsRequiredInput, out int numberOfDecisionsRequired);

            Console.Write("Enter Approval Owner User ID:");
            string ownerUserId = Console.ReadLine();

            var filePath = Path.Combine("dummy.pdf");

            var requestParameters = new ApprovalCreateParameters
            {
                Name = name,
                NumberOfDecisionsRequired = numberOfDecisionsRequired,
                OwnerUserId = ownerUserId,
            };

            Console.WriteLine("Creating new approval...");
            var approvalResult = await _apiClient.Approvals.Create(filePath, requestParameters);
            Console.WriteLine($"New approval created:{approvalResult.Id}");

        }
    }
}
