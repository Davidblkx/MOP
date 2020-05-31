using Serilog;

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
    }
}
