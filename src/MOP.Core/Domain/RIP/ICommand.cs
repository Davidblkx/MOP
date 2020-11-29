using System;
using System.Collections.Generic;

namespace MOP.Core.Domain.RIP
{
    public interface ICommand
    {
        /// <summary>
        /// Gets the name to invoke this command.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the available actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        IEnumerable<IAction> Actions { get; }

        /// <summary>
        /// Gets the command description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string? Description { get; }

        /// <summary>
        /// Gets the target service that contains the actions.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        Type Target { get; }
    }
}
