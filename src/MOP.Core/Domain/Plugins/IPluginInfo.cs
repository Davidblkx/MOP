﻿using Semver;
using System;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Plugin information
    /// </summary>
    public interface IPluginInfo
    {
        /// <summary>
        /// Gets the core version.
        /// </summary>
        /// <value>
        /// The core version.
        /// </value>
        SemVersion CoreVersion { get; }
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
        /// <summary>
        /// Gets the namespace used in properties.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        string Namespace { get; }

        /// <summary>
        /// Gets the priority, lower value have higher priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        ulong Priority { get; set; }
    }
}