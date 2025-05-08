using System;
using System.Collections.Generic;

namespace CrashKonijn.Logger
{
    public interface ILogManagerConfig
    {
        List<ILogTarget> Targets { get; set; }
        ILoggerConfig GetConfig(Type type);
        ILoggerConfig GetConfig<T>();
        ILoggerConfig GetDefaultConfig();
    }
}
