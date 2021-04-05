using MOP.Terminal.Models;
using MOP.Terminal.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Spectre.Console;

using static Spectre.Console.Color;

namespace MOP.Terminal.Services
{
    /// <summary>
    /// Handle hosts collection in settings
    /// </summary>
    internal class HostsHandlerService : IHostsHandlerService
    {
        private readonly ISettingsService _settings;

        public HostsHandlerService(ISettingsService s)
        {
            _settings = s;
        }

        /// <summary>
        /// Gets the host list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HostConfig> GetHostConfigs()
            => _settings.Hosts;

        /// <summary>
        /// Formats the host configuration output.
        /// </summary>
        /// <param name="h">The host config.</param>
        /// <returns></returns>
        public static string FormatHostConfigOutput(HostConfig h)
        {
            var parts = new string[]
            {
                h.Name.Color(Maroon),
                "=>".Color(Yellow1),
                h.Id.ToString().Color(DeepPink1),
                "@".Color(Yellow1),
                h.Hostname.Color(DeepSkyBlue1),
                ":".Color(Yellow1),
                h.Port.ToString().Color(Red3)
            };
            return string.Join("", parts);
        }

        /// <summary>
        /// Finds the host by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public HostConfig? FindHost(string name)
            => GetHostConfigs().FirstOrDefault(
                e => CompareString(e.Name, name));

        /// <summary>
        /// Finds the host by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public HostConfig? FindHost(Guid id)
            => GetHostConfigs().FirstOrDefault(e => e.Id == id);

        /// <summary>
        /// Determines whether the specified host exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified host name exists; otherwise, <c>false</c>.
        /// </returns>
        public bool HasHost(string name)
            => !(FindHost(name) is null);

        /// <summary>
        /// Removes the host.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task RemoveHost(string name)
        {
            _settings.Hosts.RemoveAll(e => CompareString(e.Name, name));
            await _settings.SaveAsync();
        }

        /// <summary>
        /// Add or update a host
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="isDefault">if set to <c>true</c> [is default].</param>
        public async Task SetHost(IHostConfig config, bool isDefault)
        {
            var value = HostConfig.From(config);

            var hostList = GetHostConfigs().ToList();
            hostList.RemoveAll(e => e.Name == value.Name);
            hostList.Add(value);
            _settings.Hosts = hostList;

            if (isDefault || hostList.Count == 1)
                _settings.DefaultHost = value.Name;

            await _settings.SaveAsync();
        }

        private static bool CompareString(string? a, string? b)
            => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }
}
