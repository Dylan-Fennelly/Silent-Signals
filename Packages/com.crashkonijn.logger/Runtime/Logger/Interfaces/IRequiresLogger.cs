namespace CrashKonijn.Logger
{
    public interface IRequiresLogger : IHasLogger
    {
        new IRabbitLogger Logger { get; set; }
    }
}
