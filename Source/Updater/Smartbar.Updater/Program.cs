namespace Smartbar.Updater
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.Threading.Tasks;
    using Smartbar.Updater.Core;

    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Smartbar update started!");

            var compositionContainer = new CompositionContainer(new ApplicationCatalog());

            Update currentUpdate = null;
            try
            {
                var smartbarUpdater = compositionContainer.GetExportedValue<ISmartbarUpdater>();
                currentUpdate = Task.Run(async () => await smartbarUpdater.GetUpdateAsync()).Result;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error is occured while initializing the update procedure!");
                Console.Read();
                Environment.Exit(0);
            }

            if (currentUpdate.Local == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error is occured while initializing the update procedure!");
                Console.Read();
                Environment.Exit(0);
            }

            if (currentUpdate.Remote == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You are using the latest version of Smartbar!");
                Console.WriteLine("Would you like to start Smartbar now? Press Y to proceed or N to abort.");

                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    currentUpdate.StartSmartbar();
                }

                Environment.Exit(0);
            }

            Console.WriteLine($"Current Smartbar version: {currentUpdate.Local.Version}");
            Console.WriteLine($"Highest available Smartbar version: {currentUpdate.Remote.Version}");

            Console.WriteLine("Would you like to proceed with the update? Press Y to proceed or N to abort.");
            if (Console.ReadKey().Key != ConsoleKey.Y)
            {
                Environment.Exit(0);
            }

            try
            {
                Task.Run(async () => await currentUpdate.InstallAsync()).Wait();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You are now using the latest version of Smartbar!");
                Console.WriteLine("Would you like to start Smartbar now? Press Y to proceed or N to abort.");

                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    currentUpdate.StartSmartbar();
                }

                Environment.Exit(0);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error is occured during the update procedure!");
                Console.Read();
                Environment.Exit(0);
            }
        }
    }
}
