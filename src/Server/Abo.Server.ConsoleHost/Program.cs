using System;
using System.Threading.Tasks;
using Abo.Server.Infrastructure.Transport;
using Abo.Utils.Logging;
using Abo.Utils.Extensions;
using Autofac;

namespace Abo.Server.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Bootstrapper.Run();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_OnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_OnUnobservedTaskException;
            Console.WriteLine("Bootstrapper is ran.");
            string command = "";
            while (command != "exit")
            {
                if (command == "stat")
                {
                    var measurer = container.Resolve<TransportPerformanceMeasurer>();
                    Console.WriteLine("Sent: {0} ({2} commands),  Received: {1} ({3} commands). MaxCommandSize: {4}, TotalConnections: {5}",
                        StringExtensions.ToShortSizeInBytesString(measurer.SentBytes),
                        StringExtensions.ToShortSizeInBytesString(measurer.ReceivedBytes), 
                        measurer.SentCommands, measurer.ReceivedCommands, StringExtensions.ToShortSizeInBytesString(measurer.MaxCommandSize), measurer.TotalConnections);
                }
                command = Console.ReadLine();
            }
        }

        private static void TaskScheduler_OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            LogFactory.GetLogger<Program>().Exception(e.Exception);
        }

        private static void CurrentDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogFactory.GetLogger<Program>().Exception(e.ExceptionObject as Exception);
        }
    }
}
