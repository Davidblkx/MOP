using MOP.Infra.Extensions;
using MOP.Infra.Tools;
using MOP.Host.Domain;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MOP.Host.Services
{
    internal class HostPropertiesService
    {
        private const string DIR_NAME = "MOP";
        private const string TEMP_DIR_NAME = "temp";
        private const string DATA_DIR_NAME = "data";

        private readonly FileInfo _file;

        public static Task<HostProperties> LoadHostProperties(string[] args)
            => new HostPropertiesService(
                PropertiesFileService.GetPropertiesFile(args)
            ).BuildProperties();

        public HostPropertiesService(FileInfo file)
        {
            _file = file;
        }

        public async Task<HostProperties> BuildProperties()
        {
            var props = await LoadPropertiesFile();
            props = FillMissingProps(props);
            return await SaveProperties(props);
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

        private async Task<HostProperties> LoadPropertiesFile()
        {
            Console.WriteLine($"Loading start properties from: {_file.FullName}");
            if (_file.Exists) return await ReadPropertiesFile(_file);
            return new HostProperties();
        }

        private async Task<HostProperties> SaveProperties(HostProperties prop)
        {
            var filePath = _file.FullName;
            var jsonBody = JsonConvert.SerializeObject(prop, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, jsonBody, Encoding.UTF8);
            return prop;
        }

        private async Task<HostProperties> ReadPropertiesFile(FileInfo file)
        {
            var jsonBody = await File.ReadAllTextAsync(file.FullName, Encoding.UTF8);
            return DeserializeHost(jsonBody);
        }

        private HostProperties DeserializeHost(string jsonBody)
        {
            return JsonConvert
                .DeserializeObject<HostProperties>(jsonBody);
        }
    }
}
