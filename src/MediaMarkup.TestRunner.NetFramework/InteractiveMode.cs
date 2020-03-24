using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMarkup.TestRunner.NetFramework
{
    internal class InteractiveMode
    {
        private readonly bool _enabled;

        internal InteractiveMode(bool enabled)
        {
            _enabled = enabled;
        }

        internal void Run()
        {
            if (!_enabled) return;

            Printer.Print("Press enter to continue");
            Console.ReadLine();
        }
    }
}
