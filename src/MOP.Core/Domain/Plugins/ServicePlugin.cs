using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MOP.Core.Domain.Plugins
{
    /// <summary>
    /// Base for a singleton service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MOP.Core.Domain.Plugins.IServicePlugin" />
    public abstract class SingletonServicePlugin<T> : IServicePlugin
    {
        public virtual LifeCycle LifeCycle => LifeCycle.Singleton;

        public virtual IEnumerable<Type> Implements
            => new List<Type> { typeof(T) };

        public virtual IPluginInfo Info { get; protected set; }

        public SingletonServicePlugin() { Info = BuildInfo(); }

        public virtual void Dispose() { }
        public virtual Task<bool> Initialize() => Task.Run(() => true);

        protected abstract IPluginInfo BuildInfo();
    }

    /// <summary>
    /// Base for a transient service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="MOP.Core.Domain.Plugins.IServicePlugin" />
    public abstract class TransientServicePlugin<T> : IServicePlugin
    {
        public virtual LifeCycle LifeCycle => LifeCycle.Transient;

        public virtual IEnumerable<Type> Implements
            => new List<Type> { typeof(T) };

        public virtual IPluginInfo Info { get; protected set; }

        public TransientServicePlugin() { Info = BuildInfo(); }

        public virtual void Dispose() { }
        public virtual Task<bool> Initialize() => Task.Run(() => true);

        protected abstract IPluginInfo BuildInfo();
    }
}
