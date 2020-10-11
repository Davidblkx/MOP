using Serilog;
using System;

namespace MOP.Core.Services
{
    /// <summary>
    /// allow to get the logger
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <returns></returns>
        ILogger GetLogger();

        /// <summary>
        /// Gets the context logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ILogger GetContextLogger<T>();

        /// <summary>
        /// Adds the logger transformer, to change the logger configuration.
        /// </summary>
        /// <param name="fn">The function.</param>
        void AddLoggerTransformer(Func<LoggerConfiguration, LoggerConfiguration> fn);
    }
}
