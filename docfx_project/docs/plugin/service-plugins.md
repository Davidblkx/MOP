# MOP Service Plugins

## About

Service plugins are classes that implement logic to be used by other plugins, 
they are injected into the plugins at runtime and there are two life cycles
available:

- Singleton, a instance is only created one time. Tra
- Transient, a instance is created at every request

## IServicePlugin

In order for a class to be used as a service it needs to implement the
`IServicePlugin`, where it will define the life cycle strategy to use
and the types that it will inject. There are two helper class to implement
Singleton and Transient without any boilerplate

### IServicePlugin example

```C#
public MyService : IType1, IType2, IServicePlugin
{
    public LifeCycle LifeCycle => LifeCycle.Transient;

    public IEnumerable<Type> Implements
            => new List<Type> { typeof(MyService), typeof(IType1), typeof(IType2) };

    public MyService(Dependency1 dep1, Dependency2 dep2) 
    {
        // ... use dependencies injected at instantiation time
    }

    // ... IType1 and IType2 logic
}
```

In this example we have a service that implements multiple interfaces to be used,
this shouldn't be the most common case but is a way to show how to define it.

### Singleton example

```C#
public MyService : SingletonServicePlugin<IType1>, IType1
{
    public MyService(Dependency1 dep1, Dependency2 dep2) 
    {
        // ... use dependencies injected at instantiation time
    }

    // ... IType1 logic
}
```

A Singleton service using the helper `SingletonServicePlugin<T>`

### Transient example

```C#
public MyService : TransientServicePlugin<IType1>, IType1
{
    public MyService(Dependency1 dep1, Dependency2 dep2) 
    {
        // ... use dependencies injected at instantiation time
    }

    // ... IType1 logic
}
```

A Transient service using the helper `TransientServicePlugin<T>`