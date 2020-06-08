using MOP.Host.Domain;
using MOP.Host.Helpers;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MOP.Host.Services
{
    internal class HostPropertiesService
    {
        private const string FILE_NAME = "mop.props.json";
        private const string DIR_NAME = "MOP";
        private const string TEMP_DIR_NAME = "temp";
        private const string DATA_DIR_NAME = "data";

        public static Task<HostProperties> LoadHostProperties()
            => new HostPropertiesService().BuildProperties();

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
            => PathHelpers.GetUserHome()
                .RelativeDirectory(DIR_NAME)
                .CreateIfRequired();

        private async Task<HostProperties> LoadPropertiesFile()
        {
            var file = GetPropertiesFile(GetAssemblyDirectory());
            Console.WriteLine($"Loading start properties from: {file.FullName}");
            if (file.Exists) return await ReadPropertiesFile(file);
            return new HostProperties();
        }

        private async Task<HostProperties> SaveProperties(HostProperties prop)
        {
            var filePath = GetPropertiesFile(GetAssemblyDirectory()).FullName;
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

        private FileInfo GetPropertiesFile(DirectoryInfo directory)
        {
            var filePath = Path.Combine(directory.FullName, FILE_NAME);
            return new FileInfo(filePath);
        }

        private DirectoryInfo GetAssemblyDirectory()
        {
            var location = Process.GetCurrentProcess().MainModule.FileName;
            var file = new FileInfo(location);
            if (!file.Exists) {
                throw new AccessViolationException("Can't locate running assembly location");
            }

            return file.Directory;
        }
    }
}
