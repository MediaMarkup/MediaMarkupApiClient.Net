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

        internal async Task GetApproval()
        {
            Printer.PrintStepTitle("Get Approval Details");
            Console.Write("Enter Approval ID:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            Console.WriteLine($"Reading approval {id}..");
            var approval = await _apiClient.Approvals.Get(id);

            Printer.PrintApproval(approval);
        }

        internal async Task UpdateApproval()
        {
            Printer.PrintStepTitle("Update Existing Approval");
            Console.Write("Enter Approval ID:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            Console.Write("Enter New Name:");
            string name = Console.ReadLine();

            Console.Write("Enter New Owner ID:");
            string ownerId = Console.ReadLine();

            Console.Write("Set Active Status (1 or 0):");
            bool.TryParse(Console.ReadLine(), out bool active);

            var parameters = new ApprovalUpdateParameters
            {
                Active = active,
                Name = name,
                OwnerUserId = ownerId
            };

            Console.WriteLine($"Updating approval {id}...");
            var approval = await _apiClient.Approvals.Update(id, parameters);
            Printer.PrintApproval(approval);
        }

        internal async Task DeleteApproval()
        {
            Printer.PrintStepTitle("Delete Existing Approval");
            Console.Write("Enter Approval ID:");
            string id = Console.ReadLine();

            if (id == "-1") return;

            Console.WriteLine($"Deleting approval {id}...");
            await _apiClient.Approvals.Delete(id);
            Console.WriteLine($"Reading approval {id}...");

            try
            {
                var approval = await _apiClient.Approvals.Get(id);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Printer.PrintApproval(null);
            }
        }
    }
}
