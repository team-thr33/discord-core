using System;
using System.Threading;

namespace DiscordCore
{
    internal class Program
    {
        private static PluginManager manager;

        private static void Main(string[] args)
        {
            Thread worker = new Thread(PluginThread);
            worker.Start();

            while (true)
            {
                string command = Console.ReadLine().ToLowerInvariant();
                if (command == "quit")
                {
                    break;
                }
                else if (command == "reload")
                {
                    manager.Stop();
                    worker.Abort();
                    worker = new Thread(PluginThread);
                    worker.Start();
                }
            }
        }

        private static void PluginThread()
        {
            manager = new PluginManager();
            manager.Start();
        }
    }
}