using System.IO;
using System.Linq;
using System.Reflection;

namespace MOP.Core.Akka.Hocon
{
    public static class HoconFileLoader
    {
        private const string FILE_NAME = "AkkaBaseConfig.hocon";

        public static string Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(e => e.Contains(FILE_NAME));
            if (resourceName is null)
                throw new FileNotFoundException($"Can't file resource for {FILE_NAME}");

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
