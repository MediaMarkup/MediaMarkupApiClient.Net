using MediaMarkup.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    public class EndToEndTesting
    {
        private readonly ApiClient ApiClient;

        private readonly TestContainer TestContainer;
        private readonly List<Func<Task>> _programOperations;

        private int _currentStep = -1;


        internal EndToEndTesting(ApiClient apiClient)
        {
            ApiClient = apiClient;
            TestContainer = new TestContainer();

            EndToEndUserTesting.ApiClient = ApiClient;
            EndToEndUserTesting.TestContainer = TestContainer;
            EndToEndApprovalTesting.ApiClient = ApiClient;
            EndToEndApprovalTesting.TestContainer = TestContainer;

            _programOperations = new List<Func<Task>>
            {
                { EndToEndUserTesting.CreateUser },
                { EndToEndUserTesting.GetUserById },
                { EndToEndUserTesting.UpdateUser },
                { EndToEndUserTesting.GetUserByEmail },
                { EndToEndApprovalTesting.CreateApproval },
                { EndToEndApprovalTesting.GetApproval },
                { EndToEndApprovalTesting.UpdateApproval },
                { EndToEndApprovalTesting.GetApproval },
                { EndToEndApprovalTesting.GetApprovalList },
                { EndToEndApprovalTesting.CreateApprovalVersion },
                { EndToEndApprovalTesting.SetApprovalVersionLock },
                { EndToEndApprovalTesting.SetApprovalVersionUnLock },
                { EndToEndApprovalTesting.CreateApprovalGroup },
                { EndToEndApprovalTesting.SetApprovalGroupReadOnly },
                { EndToEndApprovalTesting.UpdateApprovalGroup },
                { EndToEndApprovalTesting.UpsertApprovalGroupUser },
                { EndToEndApprovalTesting.UpdateApprovalGroupUserDecision },
                { EndToEndApprovalTesting.GetApproval },
                { EndToEndApprovalTesting.ResetApprovalGroupUserDecision },
                { EndToEndApprovalTesting.UpdateApprovalGroupUserDecision },
                { EndToEndApprovalTesting.CreateApprovalGroups },
                { EndToEndApprovalTesting.ExportApprovalReport },
                { EndToEndApprovalTesting.CreatePersonalUrl },
                { EndToEndApprovalTesting.ResetAllApprovalGroupDecisions },
                { EndToEndApprovalTesting.DeleteApprovalGroupUser },
                { EndToEndApprovalTesting.DeleteApprovalVersion },
                { EndToEndApprovalTesting.CreateApprovalDraft },
                { EndToEndApprovalTesting.UploadFileToApprovalDraft }
            };
        }

        private async Task Cleanup()
        {
            Printer.PrintStepTitle("Cleaning up......");
            Printer.Print("Deleting user...");

            await ApiClient.Users.Delete(TestContainer.User.Id, true);

            Printer.PrintStepTitle("Delete Existing Approval");

            Printer.Print("Deleting approval...");
            await ApiClient.Approvals.Delete(TestContainer.Approval.Id);

            Printer.Print($"Approval deleted!");

            Printer.Print("End of test....");

            _currentStep = -1;
        }

        public async Task RunEndToEndTest()
        {
            try
            {
                foreach (var program in _programOperations)
                {
                    PrintProgress();
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    await program();
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    _currentStep++;
                }
            
                await Cleanup();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await Cleanup();
            }
        }

        private void PrintProgress()
        {
            Console.Clear();
            for (int i = 0; i < _programOperations.Count; i++)
            {
                var hasRun = _currentStep >= i ? "[✓]" : "[ ]";
                Console.WriteLine($"{hasRun} {_programOperations[i].Method.Name}");
            }
        }
    }
}
