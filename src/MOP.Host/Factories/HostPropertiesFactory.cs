using MOP.Host.Domain;
using MOP.Core.Infra.Extensions;
using Optional.Unsafe;
using System;
using System.IO;
using System.Threading.Tasks;
using MOP.Core.UserSettings;
using MOP.Core.Infra.Tools;

namespace MOP.Host.Factories
{
    public class HostPropertiesFactory
    {
        private const string DIR_NAME = "MOP";
        private const string TEMP_DIR_NAME = "temp";
        private const string DATA_DIR_NAME = "data";

        private readonly IUserSettingsLoader<HostProperties> _loader;

        public HostPropertiesFactory(string[] args)
        {
            var file = PropertiesFileFactory.BuildPropertiesFile(args);
            _loader = BuildLoader(file);
        }

        public async Task<HostProperties> Build()
            => (await _loader.Load())
                .Map(e => FillMissingProps(e))
                .ValueOrFailure();

        private IUserSettingsLoader<HostProperties> BuildLoader(FileInfo file)
        {
            var defaultValue = FillMissingProps(new HostProperties());
            var factory = UserSettingsFactory
                .Create<HostProperties>()
                .SetDefaultValue(defaultValue)
                .SetFile(file);

            return factory.Build();
        }

        private HostProperties FillMissingProps(HostProperties props)
        {
            if (string.IsNullOrEmpty(props.TempDirectory))
                props.TempDirectory = GetSubRootDir(props.Id, TEMP_DIR_NAME);
            if (string.IsNullOrEmpty(props.DataDirectory))
                props.DataDirectory = GetSubRootDir(props.Id, DATA_DIR_NAME);
            return props;
        }

        private string GetSubRootDir(Guid id, string name)
            => GetRootDirectory(id)
                .RelativeDirectory(name)
                .CreateIfRequired().FullName;
        private DirectoryInfo GetRootDirectory(Guid id)
            => GetUserMopDirectory()
                .RelativeDirectory(id.ToString())
                .CreateIfRequired();

        private DirectoryInfo GetUserMopDirectory()
            => PathTools.GetUserHome()
                .RelativeDirectory(DIR_NAME)
                .CreateIfRequired();

        public static Task<HostProperties> BuildPropertiesAsync(string[] args)
            => new HostPropertiesFactory(args).Build();
    }
}
