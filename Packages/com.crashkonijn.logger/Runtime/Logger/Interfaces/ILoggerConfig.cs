namespace CrashKonijn.Logger
{
    public interface ILoggerConfig
    {
        public DebugSeverity MinimumSeverity { get; }
        public int MaxLogSize { get; }
        public bool CanLogToConsole { get; }
    }
}
