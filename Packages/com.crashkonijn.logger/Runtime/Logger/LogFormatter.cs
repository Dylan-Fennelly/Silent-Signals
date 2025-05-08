using UnityEngine;

namespace CrashKonijn.Logger
{
    public class LogFormatter
    {
        private readonly TimeHandler _timeHandler;

        public LogFormatter(TimeHandler timeHandler)
        {
            this._timeHandler = timeHandler;
        }

        public string FormatLog(string message, DebugSeverity severity)
        {
            return $"<color={this.GetColor(severity)}>[{this._timeHandler.GetTime()}]</color>: {message}";
        }

        public string FormatConsole(IRabbitLogger logger, Log log)
        {
            return $"{log.message}\n[logger/{logger.Path}/{logger.Name}]";
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

    public class TimeHandler
    {
        private string time;
        private int frame = -1;

        public string GetTime()
        {
            this.Update();

            return this.time;
        }

        private void Update()
        {
            if (Time.frameCount == this.frame)
                return;

            this.frame = Time.frameCount;
            this.time = FormatTime(Time.time);
        }

        public static string FormatTime(float timeInSeconds)
        {
            // Ensure no negative values (in case of edge cases)
            timeInSeconds = Mathf.Max(0, timeInSeconds);

            // Calculate hours, minutes, and seconds
            var hours = Mathf.FloorToInt(timeInSeconds / 3600);
            var minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
            var seconds = Mathf.FloorToInt(timeInSeconds % 60);

            // Format the result as HH:mm:ss
            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}
