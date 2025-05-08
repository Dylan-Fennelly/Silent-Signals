using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class LogsTable : VisualElement
    {
        private readonly bool showOwner;
        private IRabbitLogger[] loggers = Array.Empty<IRabbitLogger>();

        private IRabbitLogger[] Loggers
        {
            get => this.loggers;
            set
            {
                if (this.loggers == value)
                    return;

                foreach (var logger in this.loggers)
                {
                    logger.OnLog.RemoveListener(this.OnLog);
                }

                foreach (var logger in value)
                {
                    logger.OnLog.AddListener(this.OnLog);
                }

                this.loggers = value;

                this.UpdateLogView();
            }
        }

        private MultiColumnListView logViews;
        private List<Log> logs = new();
        private Dictionary<int, IRabbitLogger> loggersById;

        public LogsTable(bool showOwner)
        {
            this.showOwner = showOwner;
        }

        private void BuildTable()
        {
            this.Clear();

            // Log View
            this.logViews = new MultiColumnListView
            {
                bindingPath = "logs",
            };

            this.logViews.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.clickCount != 2)
                    return;

                var row = evt.target as VisualElement;
                if (row == null)
                    return;

                // Find the index of the clicked row
                var rowIndex = this.logViews.selectedIndex;
                var log = this.logs[rowIndex];

                if (string.IsNullOrEmpty(log.callerFilePath))
                    return;

                InternalEditorUtility.OpenFileAtLineExternal(log.callerFilePath, log.callerLineNumber);
            });

            this.logViews.columns.Add(new Column
            {
                title = "Id",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = this.logs[index].id.ToString();
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                },
                width = 50,
                sortable = true,
            });

            this.logViews.columns.Add(new Column
            {
                title = "Owner",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;

                    if (!this.loggersById.TryGetValue(this.logs[index].owner, out var logger))
                    {
                        label.text = "Logger not found";
                        return;
                    }

                    label.text = logger.Name;
                },
                width = 200,
                sortable = false,
                visible = this.showOwner,
            });

            this.logViews.columns.Add(new Column
            {
                title = "Frame",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = this.logs[index].frame.ToString();
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.color = this.GetColor(this.logs[index].severity);
                },
                width = 80,
                sortable = false,
            });

            this.logViews.columns.Add(new Column
            {
                title = "Time",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = this.logs[index].time;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.color = this.GetColor(this.logs[index].severity);
                },
                width = 80,
                sortable = false,
            });

            this.logViews.columns.Add(new Column
            {
                title = "Severity",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = this.logs[index].severity.ToString();
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.color = this.GetColor(this.logs[index].severity);
                },
                width = 50,
                sortable = false,
            });

            this.logViews.columns.Add(new Column
            {
                title = "Message",
                stretchable = true,
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = this.logs[index].message;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.paddingLeft = 5;
                },
            });

            this.Add(this.logViews);
        }

        public void SetLoggers(IRabbitLogger[] loggers)
        {
            this.BuildTable();
            this.Loggers = loggers;
        }

        private Color GetColor(DebugSeverity severity)
        {
            switch (severity)
            {
                default:
                case DebugSeverity.Log:
                    return Color.white;
                case DebugSeverity.Warning:
                    return Color.yellow;
                case DebugSeverity.Error:
                    return Color.red;
            }
        }

        private void OnLog(Log log)
        {
            this.UpdateLogView();
        }

        private void UpdateLogView()
        {
            this.loggersById = this.Loggers.ToDictionary(x => x.Id, x => x);
            this.logs = this.Loggers.SelectMany(x => x.Logs).OrderByDescending(x => x.frame).ToList();
            this.logViews.itemsSource = this.logs;
            this.logViews.RefreshItems();
        }
    }
}
