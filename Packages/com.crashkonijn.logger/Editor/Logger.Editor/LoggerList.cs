using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class LoggerList : VisualElement
    {
        private readonly TextField textField;
        private readonly ListView listView;
        private PathLoggers[] loggers;
        private string searchFilter;

        public LoggerList(Action<IRabbitLogger[]> onLoggersSelected)
        {
            this.textField = new TextField
            {
                style =
                {
                    marginBottom = 10,
                },
            };

            this.textField.RegisterValueChangedCallback(this.OnSearchChanged);

            this.Add(this.textField);

            this.listView = new ListView
            {
                makeItem = this.MakeItem,
                bindItem = this.BindItem,
                itemsSource = new List<IRabbitLogger>(),
            };

            this.listView.selectionChanged += objects =>
            {
                onLoggersSelected(objects.Cast<PathLoggers>().SelectMany(x => x.Loggers).ToArray());
            };

            this.Add(this.listView);

            LogManager.OnLoggerRegistered.AddListener(this.OnLoggerAdded);
            LogManager.OnLoggerUnregistered.AddListener(this.OnLoggerRemoved);

            this.Update();
        }

        private void OnLoggerRemoved(IRabbitLogger logger)
        {
            this.Update();
        }

        private void OnLoggerAdded(IRabbitLogger logger)
        {
            this.Update();
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            this.searchFilter = evt.newValue.ToLower();
            this.Update();
        }

        private void Update()
        {
            this.UpdateList(this.GetFilteredLoggers());
        }

        private PathLoggers[] GetFilteredLoggers()
        {
            if (!LogManager.HasInstance)
                return Array.Empty<PathLoggers>();

            if (string.IsNullOrEmpty(this.searchFilter))
                return LogManager.Instance.Loggers
                    .GroupBy(x => x.Path)
                    .Select(x => new PathLoggers { Path = x.Key, Loggers = x.ToList() })
                    .ToArray();

            return LogManager.Instance.Loggers
                .GroupBy(x => x.Path)
                .Where(group => group.Key.ToLower().Contains(this.searchFilter))
                .Select(x => new PathLoggers { Path = x.Key, Loggers = x.ToList() })
                .ToArray();
        }

        private void UpdateList(PathLoggers[] loggers)
        {
            this.loggers = loggers;
            this.listView.itemsSource = loggers;
            this.listView.Rebuild();
        }

        private void BindItem(VisualElement element, int index)
        {
            if (element is not Label label)
                return;

            label.text = this.loggers[index].Path;
        }

        private VisualElement MakeItem()
        {
            return new Label();
        }

        private class PathLoggers
        {
            public string Path { get; set; }
            public List<IRabbitLogger> Loggers { get; set; }
        }
    }
}
