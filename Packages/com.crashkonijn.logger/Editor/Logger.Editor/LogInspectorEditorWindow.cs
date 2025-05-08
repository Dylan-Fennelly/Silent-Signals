using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class LogInspectorEditorWindow : EditorWindow
    {
        private IRabbitLogger[] selectedLoggers = Array.Empty<IRabbitLogger>();

        private IRabbitLogger[] SelectedLoggers
        {
            get => this.selectedLoggers;
            set
            {
                if (this.selectedLoggers == value)
                    return;

                foreach (var logger in this.selectedLoggers)
                {
                    logger.OnLog.RemoveListener(this.OnLog);
                }

                foreach (var logger in value)
                {
                    logger.OnLog.AddListener(this.OnLog);
                }

                this.selectedLoggers = value;

                this.UpdateLogView();
            }
        }

        private List<Log> logs = new();

        private MultiColumnListView logViews;

        [MenuItem("Tools/Logger/Inspector #l")]
        public static void ShowWindow()
        {
            var window = GetWindow<LogInspectorEditorWindow>();
            window.titleContent = new GUIContent("Log Inspector");

            window.OnSelectionChanged();
        }

        private void OnEnable()
        {
            Selection.selectionChanged += this.OnSelectionChanged;
            EditorApplication.playModeStateChanged += this.LogPlayModeState;

            var root = this.rootVisualElement;

            // Log View
            this.logViews = new MultiColumnListView
            {
                bindingPath = "logs",
                style =
                {
                    flexGrow = 1,
                    borderTopColor = Color.gray,
                    borderTopWidth = 1,
                    paddingTop = 5,
                },
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
                Debug.Log($"Double-clicked row: {rowIndex}");
                var log = this.logs[rowIndex];

                if (string.IsNullOrEmpty(log.callerFilePath))
                    return;

                InternalEditorUtility.OpenFileAtLineExternal(log.callerFilePath, log.callerLineNumber);
            });

            this.logViews.columns.Add(new Column
            {
                title = "Owner",
                makeCell = () => new Label(),
                bindCell = (element, index) =>
                {
                    var label = element as Label;
                    label.text = LogManager.Instance.LoggersById[this.logs[index].owner].Name;
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                },
                width = 150,
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

            root.Add(this.logViews);
        }

        public Color GetColor(DebugSeverity severity)
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

        private void OnDisable()
        {
            Selection.selectionChanged -= this.OnSelectionChanged;
            EditorApplication.playModeStateChanged -= this.LogPlayModeState;
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeGameObject == null)
                return;

            if (!LogManager.HasInstance)
                return;

            var loggables = LogManager.Instance.Loggers.OfType<IGameObjectLogger>().Where(x => x.GameObject == Selection.activeGameObject).ToArray();

            if (loggables.Length == 0)
            {
                this.SelectedLoggers = Array.Empty<IRabbitLogger>();
                return;
            }

            this.SelectedLoggers = loggables;
        }

        private void LogPlayModeState(PlayModeStateChange evt)
        {
            if (evt == PlayModeStateChange.EnteredEditMode)
            {
                this.SelectedLoggers = Array.Empty<IRabbitLogger>();
            }

            if (evt == PlayModeStateChange.EnteredPlayMode)
            {
                this.OnSelectionChanged();
            }
        }

        private void OnLog(Log log)
        {
            this.UpdateLogView();
        }

        private void UpdateLogView()
        {
            this.logs = this.SelectedLoggers.SelectMany(x => x.Logs).OrderBy(x => x.frame).ToList();

            this.logViews.itemsSource = this.logs;
            this.logViews.RefreshItems();
        }
    }
}
