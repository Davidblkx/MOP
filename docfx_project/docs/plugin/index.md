# MOP Plugins

## About

Plugins allow to add new functionality to the Host. In MOP, plugins are develop
using any .NET language, and to install them you just need to copy the file to 
the right folder. There are 2 kind of plugins, it can be a class 
that extends the interface `MOP.Core.Domain.Plugins.IPlugin` or a class that has 
the attribute `MOP.Core.Domain.Plugins.InjectableAttribute`

## IPlugin

`IPlugin` should be used when you intend to have functionality that is 
supposed to be used by th end user. This kind of plugins are only 
instantiated one time and is the developer responsibility to handle
errors and control interaction.

### IPluginExample

```C#
public class HelloWorldPlugin : IPlugin
{

    public IPluginInfo Info => new MyPluginInfo();

    private ILogger Logger { get; }

    public HelloWorldPlugin(ILogService logService)
    {
        Logger = logService.GetContextLogger<HelloWorldPlugin>();
    }

    public async Task<bool> Initialize()
    {
        if (Logger is null) return false;
        Logger.Information("Hello world from plugin!");
        return true;
    }

    public void Dispose() {}
}
```

This is just a simple plugin that logs a message when is loaded.
`IPluginInfo` is the information related to the plugin you can 
check more details below. The constructor can have dependencies 
that will be injected at instantiation time. `Task<bool> Initialize()`
is called after are plugins are instantiated. 

## IPluginInfo

`IPluginInfo` is responsible to tell the host how to identify and 
when to load a plugin. It also contains the minimum target version 
of the host that is required to run

### IPluginInfo Example

```C#
public class MyPluginInfo : IPluginInfo
{
    public Guid Id => Guid.Parse("cb3664b5-0b76-4a59-a018-acfcf8ad15ae");
    public string Name => "My super duper plugin";
    public string Namespace => "my.plugin";
    public MopVersion CoreVersion => new MopVersion(0,1,0);
    public ulong Priority => PluginPriority.DEFAULT;
    public MopVersion Version => new MopVersion(0,1,0);
}
```

- `Id` is a GUID constant that identifies the plugin needs to be unique
- `Name` is a human readable form to identify the plugin, can be the same for multiple plugins.
- `Namespace` prefix to use when storing properties for a plugin
- `CoreVersion` minimum host version to use
- `Priority` number that sets the order in which is loaded, lower numbers are loaded first
- `Version` plugin version


## BasePlugin<T>

Since most plugins will required some basic dependencies like a logger
or to subscribe to events a base implementation for `IPlugin` exists.
This simplifies the boilerplate to create a new plugin

### BasePlugin<T> Example

```C#
public class TriggerPlugin : BasePlugin<TriggerPlugin>
{
    public BaseActorPlugin(IInjectorService injector) : base(injector) {}

    protected async override Task<bool> OnInitAsync()
    {
        await Subscribe(OnPlay, "music.play.start", "music.play.stop");
        return true;
    }

    protected override IPluginInfo BuildPluginInfo()
        => new TriggerPluginInfo();

    private void OnPlay(IEvent e)
    {
        switch(e)
        {
            case PlayStartEvent start:
                Logger.Information($"Start playing a new song at {start.DateTime}");
                break;
            case PlayStopEvent stop:
                Logger.Information($"Stopped playback at {stop.DateTime}");
                break;
        }
    }
}
```

This plugin logs a message when a event of type `music.play.start` or `music.play.stop` is emit.

