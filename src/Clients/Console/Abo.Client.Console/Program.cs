using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Abo.Client.Core.Managers;
using Abo.Client.Core.Model.Enums;
using Autofac;

namespace Abo.Client.Console
{
    class Program
    {
        private static readonly Random Random = new Random();

        static void Main()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ThreadPool.SetMinThreads(100, 10);
            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(_ => { });
            }
            sw.Stop();
            System.Console.WriteLine("{0} ticks", sw.ElapsedMilliseconds);
            return;

            var maxLen = BitConverter.GetBytes(int.MaxValue - 1);
            ThreadPool.QueueUserWorkItem(o => RunAsync());
            Thread.Sleep(int.MaxValue);
        }
        
        private static async void RunAsync()
        {
            var container = Bootstrapper.Run();
            var appManager = container.Resolve<AppManager>();
            var accountManager = container.Resolve<AccountManager>();
            var chatManager = container.Resolve<ChatManager>();

            if (await appManager.InitAsync() != AuthenticationResult.Success)
            {
                await accountManager.Register("ConsoleClient_" + Random.Next(0, 9999), "123", 24, true, "BELARUS", -25);
            }

            await chatManager.ReloadChat();
            await chatManager.ReloadPlayers();

            string command = System.Console.ReadLine();
            while (command != "exit")
            {
                chatManager.SendMessage(command);
                command = System.Console.ReadLine();
            }
        }
    }
}
