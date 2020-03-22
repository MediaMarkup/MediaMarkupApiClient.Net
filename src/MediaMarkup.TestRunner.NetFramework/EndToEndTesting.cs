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

        internal EndToEndTesting(ApiClient apiClient)
        {
            _apiClient = apiClient;

            _testContainer = new TestContainer();
            _userTesting = new EndToEndUserTesting(_apiClient, _testContainer);
        }

        private async Task Cleanup()
        {
            Printer.PrintStepTitle("Cleaning up......");
            Printer.Print("Deleting user...");

            await _apiClient.Users.Delete(_testContainer.User.Id, true);

            Printer.Print("End of test....");
        }

        public async Task RunEndToEndTest()
        {
            await _userTesting.Run();

            await Cleanup();
        }
    }
}
