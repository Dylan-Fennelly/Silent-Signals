using System;

namespace CrashKonijn.Logger
{
    [Serializable]
    public struct Log
    {
        public int id;
        public int owner;
        public string time;
        public int frame;
        public DebugSeverity severity;
        public string message;
        public string callerFilePath;
        public int callerLineNumber;

        public override string ToString()
        {
            return $"<color={this.GetColor(this.severity)}>{this.time} [{this.severity}]</color> {this.message}";
        }

        public string GetColor(DebugSeverity severity)
        {
            switch (severity)
            {
                case DebugSeverity.Log:
                    return "white";
                case DebugSeverity.Warning:
                    return "yellow";
                case DebugSeverity.Error:
                    return "red";
                default:
                    return "white";
            }
        }
    }
}