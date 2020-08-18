using Optional;
using System.IO;
using System.Linq;
using MOP.Infra.Extensions;

using static Optional.Option;

namespace MOP.Host.Services
{
    /// <summary>
    /// Builds the HostProperties FileInfo
    /// </summary>
    internal class PropertiesFileService
    {
        private static readonly string[] VALID_ARGS = new[] { "--config", "-c" };
        private const string FILE_NAME = ".mop.json";

        /// <summary>
        /// Gets the properties file from argument --config or -c
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static FileInfo GetPropertiesFile(string[] args)
            => new PropertiesFileService(args).GetFile();

        private readonly string[] _args;

        public PropertiesFileService(string[] args)
        {
            _args = args;
        }

        public FileInfo GetFile()
            => GetFileFromArgument()
                .ValueOr(new FileInfo(FILE_NAME));

        private Option<FileInfo> GetFileFromArgument()
            => GetFilePathFromArgument()
                .Match(
                    some: e => e.ToFileInfo(), 
                    none: () => None<FileInfo>()
                );

        private Option<string> GetFilePathFromArgument()
        {
            for(var i = 0; i < _args.Length; i++)
            {
                if (VALID_ARGS.Contains(_args[i]))
                    if (i + 1 < _args.Length)
                        return _args[i + 1].ToOptional();
            }

            return None<string>();
        }
    }
}
