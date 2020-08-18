﻿using Akka.Actor;
using MOP.Infra.Domain.Actors;
using Optional;
using System.Collections.Generic;

namespace MOP.Infra.Services
{
    /// <summary>
    /// Service handle <see cref="IActorRef"/> instances
    /// </summary>
    public interface IActorService
    {
        /// <summary>
        /// Gets the main actor system.
        /// </summary>
        /// <value>
        /// The main actor system.
        /// </value>
        public ActorSystem MainActorSystem { get; }

        /// <summary>
        /// Gets a instance of a <see cref="IActorRef"/> for a name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Option<IActorRef> GetActorOf(string name);

        /// <summary>
        /// Gets the actor factory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Option<IActorFactory> GetActorFactory(string name);

        /// <summary>
        /// Adds the actor factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        /// <returns><c>true</c>, if factory was added</returns>
        bool AddActorFactory(IActorFactory factory, bool replace = true);

        /// <summary>
        /// Determines whether [has a actor factory] for [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [has actor factory] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        bool HasActorFactory(string name);

        /// <summary>
        /// Gets all <see cref="IActorFactory"/>
        /// </summary>
        /// <returns></returns>
        IEnumerable<IActorFactory> GetActorRefFactories();
    }
}
