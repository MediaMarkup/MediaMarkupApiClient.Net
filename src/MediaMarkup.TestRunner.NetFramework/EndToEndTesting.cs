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
        private readonly ApiClient _apiClient;

        private TestContainer _testContainer;
        private EndToEndUserTesting _userTesting;
        private EndToEndApprovalTesting _approvalTesting;
        private InteractiveMode _interactiveMode;

        private const bool InteractiveModeEnabled = true;

        internal EndToEndTesting(ApiClient apiClient)
        {
            _apiClient = apiClient;

            _interactiveMode = new InteractiveMode(InteractiveModeEnabled);
            _testContainer = new TestContainer();
            _userTesting = new EndToEndUserTesting(_apiClient, _testContainer, _interactiveMode);
            _approvalTesting = new EndToEndApprovalTesting(_apiClient, _testContainer);
        }

        private async Task Cleanup()
        {
            Printer.PrintStepTitle("Cleaning up......");
            Printer.Print("Deleting user...");

            await _apiClient.Users.Delete(_testContainer.User.Id, true);

            Printer.PrintStepTitle("Delete Existing Approval");

            Printer.Print("Deleting approval...");
            await _apiClient.Approvals.Delete(_testContainer.Approval.Id);

            Printer.Print($"Approval deleted!");

            Printer.Print("End of test....");
        }

        public async Task RunEndToEndTest()
        {
            await _userTesting.Run();
            await _approvalTesting.Run();

            await Cleanup();
        }
    }
}
