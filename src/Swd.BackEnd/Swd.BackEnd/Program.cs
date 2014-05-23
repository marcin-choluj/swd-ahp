using System;
using System.Diagnostics;
using Funq;
using ServiceStack.Configuration;
using ServiceStack.WebHost.Endpoints;

namespace Swd.BackEnd
{
    class Program
    {
        private static readonly string ListeningOn = ConfigUtils.GetAppSetting("ListeningOn");

        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:1337/" : args[0];
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}",
                DateTime.Now, listeningOn);

            Console.ReadKey();
        }
    }
}
