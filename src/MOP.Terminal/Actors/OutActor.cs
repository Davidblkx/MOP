using MOP.Terminal.Actors.Helpers;
using MOP.Terminal.Models.Messages;
using Spectre.Console;
using System;

namespace MOP.Terminal.Actors
{
    internal class OutActor : BaseMopActor
    {
        public const string ACTOR_NAME = "out";

        public OutActor()
        {
            Receive<Print>(OnPrint);
            Receive<Clear>(OnClear);
        }

        private void OnPrint(Print message)
        {
            AnsiConsole.WriteLine(message.ToPrint);
            Continue(message.Exit);
        }

        private void OnClear(Clear _)
        {
            Console.Clear();
            Continue(false);
        }

        private void Continue(bool exit)
        {
            TellActor(ActorPaths.EndActorMeta, exit);
        }
    }
}
