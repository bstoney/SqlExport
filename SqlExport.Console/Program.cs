using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlExport.Common;

namespace SqlExport
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationEnvironment.Default.InitialiseEnvironment();

            CommandLineArguments cla = new CommandLineArguments();
            cla.Parse(args);

            (new ConsoleExport()).ExecuteQuery(cla);
        }
    }
}
