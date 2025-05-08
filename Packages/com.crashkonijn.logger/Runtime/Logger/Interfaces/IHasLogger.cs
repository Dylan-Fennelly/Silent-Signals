namespace CrashKonijn.Logger
{
    public interface IHasLogger
    {
        IRabbitLogger Logger { get; }
    }
}
