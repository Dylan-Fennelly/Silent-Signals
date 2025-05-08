using System;

namespace CrashKonijn.Logger
{
    public interface ILogTarget : IDisposable
    {
        void Initialize(ILogManager manager);
        bool IsValid();
    }
}