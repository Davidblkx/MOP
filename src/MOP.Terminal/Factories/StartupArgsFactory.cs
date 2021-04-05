using MOP.Terminal.Models;
using System.Collections.Generic;

namespace MOP.Terminal.Factories
{
    /// <summary>
    /// Read the start up arguments and returns an array with the rest
    /// </summary>
    public static class StartupArgsFactory
    {
        public static (StartupArgs, string[] args) Build(string[] args)
        {
            var interactive = false;
            var settingsFile = "";

            List<string> toReturn = new();
            for(var i = 0; i < args.Length; i++)
            {
                if (args[i] == StartupArgName.Interactive || args[i] == StartupArgName.InteractiveShort)
                {
                    interactive = true;
                    continue;
                }

                if (args[i] == StartupArgName.SettingsFile)
                {
                    i++;
                    if (i >= args.Length) continue;
                    settingsFile = args[i];
                    continue;
                }

                toReturn.Add(args[i]);
            }

            return (new StartupArgs(interactive, settingsFile), toReturn.ToArray());
        }
    }
}
