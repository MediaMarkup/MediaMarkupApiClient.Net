using MediaMarkup.Api.Models;
using Newtonsoft.Json;
using System;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal static class Printer
    {
        internal static void PrintStepTitle(string title)
        {
            Console.WriteLine($"\n{title}\n-------------");
        }

        internal static void PrintUser(User user)
        {
            if (user == null)
            {
                Console.WriteLine("User not found!");
                return;
            }

            Console.WriteLine("\n-------------------------");
            Console.WriteLine("User Details:");
            Console.WriteLine($"ID  :{user.Id}");
            Console.WriteLine($"Name:{user.FirstName} {user.LastName}");
            Console.WriteLine("-------------------------\n");
        }

        internal static void PrintApproval(Approval approval)
        {
            if (approval == null)
            {
                Console.WriteLine("Approval not found!");
                return;
            }

            var response = JsonConvert.SerializeObject(approval);
            Console.WriteLine("\n-------------------------");
            Console.WriteLine("Approval Details:");
            Console.WriteLine(response);
            Console.WriteLine("-------------------------\n");
        }
    }
}
