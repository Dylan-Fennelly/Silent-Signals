# Logger Factory

The `LoggerFactory` is a factory class that is responsible for creating loggers. It creates a new logger based on the type of the class that is requesting it.

Each factory implements the following interface:

```csharp
public interface IRabbitLoggerFactory
{
    IRabbitLogger Create(GameObject gameObject, string name, string path = "", ILoggerConfig config = null);
    IRabbitLogger Create<T>(GameObject gameObject, string path = "", ILoggerConfig config = null);
    IRabbitLogger Create<T>(string name = null, string path = null, ILoggerConfig config = null)
        where T : class;
    IRabbitLogger Create<T>(T instance, string name = null, string path = null, ILoggerConfig config = null)
        where T : class;
}
```

## Creating a logger
You can access the factories in the following ways:
```csharp
// Create a new logger using the factory.
var logger = LoggerFactory.Create(this);

// Acces the logger through the factory.
var logger = LogManager.Instance.Factory.Create(this);
```

## IDisposable
Each logger needs to be disposed of when it's no longer needed. This is done by calling the `Dispose` method on the logger. This will remove the logger from the `LogManager` and clean up any resources that the logger is using.

```csharp
private void OnDestroy() {
    logger.Dispose();
}
```

## Logger Factory Behaviour
The `LoggerFactoryBehaviour` is a helper class that can keep track of all the loggers that are created on a `GameObject`. It will dispose of all the loggers when the `GameObject` is destroyed.

### Injection
The `LoggerFactoryBehaviour` will inject a logger into any class on the GameObject that extends `IRequiresLogger` interface. This is how the `LoggerBehaviour` works. You can use this to inject a logger into any class.

```csharp
public class ExampleLogBehaviour : MonoBehaviour, IRequiresLogger {
    [field: SerializeReference]
    public IRabbitLogger Logger { get; set; }

    private void Start()
    {
        Logger.Log("Hello, World!");
    }    
}
```

### Factory
The `LoggerFactoryBehaviour` als implements the `IRabbitLoggerFactory` interface. This allows you to create loggers using the factory.

```csharp
var logger = GetComponent<LoggerFactoryBehaviour>().Create(this);
```