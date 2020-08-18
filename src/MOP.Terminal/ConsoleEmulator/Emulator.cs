using MOP.Infra.Extensions;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace MOP.Terminal.ConsoleEmulator
{
    public static class Emulator
    {
        public static async Task Execute()
        {
            if (CommonParser is null)
                throw new Exception("Parser not set");

            PrintWelcome();
            while (true)
            {
                PrintCaret();
                var input = Console.ReadLine();
                if (IsReserved(input)) continue;
                if (input.EqualIgnoreCase("exit")) break;
                if (input.EqualIgnoreCase("help")) input = "--help";
                if (input.EqualIgnoreCase("clear"))
                {
                    Console.Clear();
                    continue;
                }
                await CommonParser.InvokeAsync(input);
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
            Console.Write("|MOP|> ");
        }

        private static bool IsReserved(string input)
        {
            return input.IsNullOrEmpty()
                || input.EqualIgnoreCase("-i")
                || input.EqualIgnoreCase("--interactive");
        }

        public static Parser? CommonParser { get; set; }
    }
}
