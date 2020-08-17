using MOP.Core.Helpers;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal.ConsoleEmulator
{
    public static class Emulator
    {
        public static async Task Execute(RootCommand rootCommand)
        {
            PrintWelcome();
            while(true)
            {
                PrintCaret();
                var input = Console.ReadLine();
                if (input.IsNullOrEmpty()) continue;
                if (input.InvariantCompare("exit")) break;
                if (input.InvariantCompare("help")) input = "--help";
                if (input.InvariantCompare("clear"))
                {
                    Console.Clear();
                    continue;
                }
                await rootCommand.InvokeAsync(input);
            }
            Console.WriteLine("Exiting...");
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to MOP Terminal");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("help => show list of commands");
            Console.WriteLine("exit => exit from this app");
            Console.WriteLine("--------------------------------");
        }

        private static void PrintCaret()
        {
            Console.Write("&> ");
        }
    }
}
