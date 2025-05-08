# Log Manager
The Log Manager is a singleton that manages all the loggers in the scene. It holds a reference to all the Loggers. It will also handle the LogTargets.

## Log Manager Initializer

Before being able to log the `LogManager` needs to be initialized. This can be done using the `LogManagerInitializer`. Create a new GameObject in your scene and add the `LogManagerInitializer` component to it. Make sure to assign the created `LoggerConfigScriptable`. This will initialize the `LogManager` and make sure that all loggers are correctly set up.

{% hint style="info" %}
**IoC** This package fully supports being used in an IoC container.
{% endhint %}

## IoC Container

The package fully supports being used with an IoC container. You can register the `ILoggerFactory` and `ILogManager` interface with the container to allow for easy dependency injection.

```csharp
public LogManagerConfigScriptable logManagerConfig;

protected override void Configure(IContainerBuilder builder)
{
    // Logger
    builder.RegisterInstance<ILogManagerConfig>(logManagerConfig);
    builder.Register<IRabbitLoggerFactory, DefaultLoggerFactory>(Lifetime.Singleton);
    builder.Register<ILogManager, LogManager>(Lifetime.Singleton);
}
```