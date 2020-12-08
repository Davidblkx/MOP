using MOP.Core.Domain.RIP.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOP.RemoteInterfaceProtocol
{
    internal enum RIPErrors
    {
        /// <summary>
        /// No error code
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Error when some exception occurs
        /// </summary>
        InternalError = 666,

        /// <summary>
        /// The command not found
        /// </summary>
        CommandNotFound = 667,

        /// <summary>
        /// The action not found
        /// </summary>
        ActionNotFound = 668,
    }

    internal static class ErrorMap
    {
        private const string DETAIL_TOKEN = "#%DETAIL%";
        private const string EXCEPTION_TOKEN = "#%EXCEPTION%";
        private static Dictionary<RIPErrors, string>? _map;

        public static Response BuildError(RIPErrors code, string? details = default, Exception? ex = default)
        {
            var message = new StringBuilder(GetMapValue(code));
            message.Replace(DETAIL_TOKEN, details is null ? "" : $": {details}");
            message.Replace(EXCEPTION_TOKEN, ex is null ? "" : $"[{ex.Message}]");
            var error = ((int)code, message.ToString());
            return Response.Fail(error);
        }

        private static string GetMapValue(RIPErrors code)
        {
            if (_map is null) 
                _map = new Dictionary<RIPErrors, string>(GetErrorMessages());

            if (_map.ContainsKey(code)) return _map[code];
            return _map[RIPErrors.InternalError];
        }

        private static IEnumerable<KeyValuePair<RIPErrors, string>> GetErrorMessages()
            => new List<KeyValuePair<RIPErrors, string>> {
                BuildEntry(RIPErrors.NoError, ""),
                BuildEntry(RIPErrors.InternalError, "Internal error"),
                BuildEntry(RIPErrors.CommandNotFound, "Can't find command"),
                BuildEntry(RIPErrors.ActionNotFound, "Can't find action"),
            };

        private static string BuildBaseMessage(RIPErrors code, string baseMessage)
            => $"Error RIP{(int)code}: {baseMessage}{DETAIL_TOKEN}{EXCEPTION_TOKEN}";

        private static KeyValuePair<RIPErrors, string> BuildEntry(RIPErrors code, string message)
            => new KeyValuePair<RIPErrors, string>(code, BuildBaseMessage(code, message));
    }
}
