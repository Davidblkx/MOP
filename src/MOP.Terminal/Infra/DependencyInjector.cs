using System;
using System.CommandLine.Facilitator;
using System.Linq;
using System.Reflection;
using MOP.Core.Infra;
using MOP.Terminal.Actors;
using MOP.Terminal.CommandLine;
using MOP.Terminal.Factories;
using MOP.Terminal.Models;
using MOP.Terminal.Services;
using MOP.Terminal.Services.Impl;
using SimpleInjector;

namespace MOP.Terminal.Infra
{
    /// <summary>
    /// Dependency handler
    /// </summary>
    public static class DependencyInjector
    {
        private static Container? _container;

        public static Container Container
        {
            get
            {
                return _container ?? throw new 
                    ArgumentNullException("Container was not initialized");
            }
        }

        /// <summary>
        /// Register services.
        /// </summary>
        public static string[] Register(string[] args)
        {
            _container = new Container();

            var (startup, restArgs) = StartupArgsFactory.Build(args);
            var life = new MopLifeService(AppState.Life.Token);
            var cmd = CommandLineFacilitator
                .Create(new CustomActivator())
                .AddCurrentAssembly();

            _container.Register(() => startup, Lifestyle.Singleton);
            _container.Register(() => cmd, Lifestyle.Singleton);
            _container.Register(() => life, Lifestyle.Singleton);
            _container.Register<ISettingsLoaderService<AppSettings>, SettingsLoaderService<AppSettings>>(Lifestyle.Singleton);
            _container.Register<ISettingsService, SettingsService>(Lifestyle.Singleton);
            _container.Register<ILogService, LogService>(Lifestyle.Singleton);
            _container.Register<IHostsHandlerService, HostsHandlerService>(Lifestyle.Singleton);
            _container.Register<IParserService, ParserService>(Lifestyle.Singleton);
            _container.Register<IActorService, ActorService>(Lifestyle.Singleton);
            _container.Register<ICommandLineService, CommandLineService>(Lifestyle.Singleton);

            return restArgs;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
            where T : class => Container.GetInstance<T>();
    }
}
