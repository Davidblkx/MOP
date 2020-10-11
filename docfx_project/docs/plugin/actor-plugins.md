# MOP Actor Plugins

## About

At is core, the MOP is built using a actor system, [AKKA.NET](https://getakka.net/), because of this
the best way to create a complex plugin is to use the actor system to control it. To achieve this
the service `IInjectorService` can be used to get the `ActorSystem` and then add your plugin actor or
extending the `BaseActorPlugin<T>`.

## BaseActorPlugin<T>

Abstract class to be extended by plugins that create actors, it also extends the `BasePlugin<T>`
so some boilerplate for logging and events are already there.

### BaseActorPlugin<T> example

**Take a simple actor that only logs a message:**

```C#
public class MyActor : ReceiveActor
{
    public MyActor(ILogger log)
    {
        Receive<string>(message => log.Information(message));
    }

    public static Props WithProps(ILogger log)
        => Props.Create(() => new MyActor(log));
}
```

**To create the actor into the MOP.Host ActorSystem:**

```C#
public class MyActorPlugin : BaseActorPlugin<MyActor>
{
    public override string ActorRefName => "MyActor";

    public MyActorPlugin(IInjectorService injector) : base(injector) { }

    protected override async Task<Props> GetActorPropsAsync()
        => MyActor.WithProps(Logger);
}
```

So, as simple as this a actor is created into the actor system using an external plugin