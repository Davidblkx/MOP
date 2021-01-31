using Serilog;
using System;
using System.IO;
using MOP.Core.Infra.Extensions;
using MOP.Core.Infra.Tools;
using System.Diagnostics.CodeAnalysis;

namespace MOP.Core.UserSettings
{
    public class UserSettingsFactory<T>
    {
        /// <summary>
        /// Gets or sets the folder that contains the file.
        /// </summary>
        /// <value>
        /// Starting folder as default
        /// </value>
        public DirectoryInfo Directory { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// defaults to settings.json
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public T DefaultValue { get; set; }

        internal UserSettingsFactory()
        {
            Logger = new LoggerConfiguration().CreateLogger();
            DefaultValue = BuildDefault();
            FileName = "settings.json";
            Directory = PathTools.GetStartDirectory();
        }

        public UserSettingsFactory<T> SetDirectory(DirectoryInfo dir)
        {
            Directory = dir;
            return this;
        }

        public UserSettingsFactory<T> SetFileName(string name)
        {
            FileName = name;
            return this;
        }

        public UserSettingsFactory<T> SetFile(FileInfo file)
        {
            if (file.Directory is null)
                throw new ArgumentNullException("Directory is not defined for " + file.FullName);
            Directory = file.Directory;
            FileName = file.Name;
            return this;
        }

        public UserSettingsFactory<T> UseUserFolder()
        {
            Directory = PathTools.GetUserHome();
            return this;
        }

        public UserSettingsFactory<T> SetLogger(ILogger logger)
        {
            Logger = logger;
            return this;
        }

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        public UserSettingsFactory<T> SetDefaultValue(T defaultValue)
        {
            if (defaultValue is null)
                throw new ArgumentNullException("Default value can't be null");
            DefaultValue = defaultValue;
            return this;
        }

        public IUserSettingsLoader<T> Build()
        {
            if (DefaultValue is null)
                throw new Exception("You need to set the default value");

            var file = Directory.RelativeFile(FileName);
            return new UserSettingsLoader<T>(file, Logger, DefaultValue);
        }

        /// <summary>
        /// tries to build the default value for T.
        /// </summary>
        /// <returns></returns>
        private static T BuildDefault()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch
            {
#pragma warning disable CS8603 // Possible null reference return.
                return default;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }

    public static class UserSettingsFactory
    {
        public static UserSettingsFactory<T> Create<T>()
            => new UserSettingsFactory<T>();
    }
}
