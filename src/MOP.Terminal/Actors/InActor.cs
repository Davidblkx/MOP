using MOP.Terminal.Actors.Helpers;
using MOP.Terminal.Infra;
using MOP.Terminal.Models;
using MOP.Terminal.Models.Messages;
using MOP.Terminal.Services;
using System;

using static MOP.Terminal.Actors.Helpers.ActorPaths;

namespace MOP.Terminal.Actors
{
    /// <summary>
    /// Actor responsible for reading user input from terminal
    /// </summary>
    /// <seealso cref="Akka.Actor.ReceiveActor" />
    internal class InActor : BaseMopActor
    {
        public const string ACTOR_NAME = "input";

        internal readonly IParserService _parser;

        public InActor()
        {
            _parser = DependencyInjector.GetInstance<IParserService>();
            Receive<bool>(OnInputRequest);
            Receive<InActorArguments>(OnInvoke);
            Receive<string>(OnPing);
        }

        /// <summary>
        /// Called when a boolean message is received
        /// 
        /// Reads from console and send it to the parser
        /// </summary>
        /// <param name="readInput">if set to <c>true</c> [read input].</param>
        /// <exception cref="ArgumentNullException">command - Input string is null</exception>
        private void OnInputRequest(bool readInput)
        {
            if(!readInput) { Clear(); return; }

            var args = Console.ReadLine();
            if (args is null) throw new ArgumentNullException("command", "Input string is null");
            else if (args == "exit") { Exit(); }
            else if (args == "clear" || args == "cls") { Clear(); }
            else if (args == "ping") { Ping(); }
            else { HandleArguments(args); }
        }

        private void OnPing(string _)
        {
            var toPrint = $"Ping from: {Sender.Path}";
            TellActor(OutActorMeta, OutActorMessages.Print(toPrint));
        }

        /// <summary>
        /// Called when an <see cref="InActorArguments"/> is received, send arguments to parser.
        /// </summary>
        /// <param name="message">The message.</param>
        private void OnInvoke(InActorArguments message)
        {
            if (message.Args.Length > 0)
                HandleArguments(message.Args);
            else
                TellActor(EndActorMeta, EndActorMessages.VALIDATE);
        }

        private void HandleArguments(string args) => SaveStatus(_parser.Invoke(args));
        private void HandleArguments(string[] args) => SaveStatus(_parser.Invoke(args));

        /// <summary>
        /// Terminates app instance.
        /// </summary>
        private void Exit() 
        {
            TellActor(EndActorMeta, EndActorMessages.TERMINATE);
        }

        /// <summary>
        /// Clears the output.
        /// </summary>
        private void Clear()
        {
            TellActor(OutActorMeta, OutActorMessages.Clear());
        }

        private void Ping()
        {
            DependencyInjector
                .GetInstance<IActorService>()
                .Ping("ping", Context.Self);
            TellActor(EndActorMeta, EndActorMessages.VALIDATE);
        }

        /// <summary>
        /// Saves the command result status.
        /// </summary>
        /// <param name="code">The code.</param>
        private void SaveStatus(int code)
        {
            AppState.Result = code;
            TellActor(EndActorMeta, EndActorMessages.VALIDATE);
        }
    }
}
