using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class LoggerOverviewEditorWindow : EditorWindow
    {
        private IRabbitLogger[] selectedLoggers = Array.Empty<IRabbitLogger>();

        private IRabbitLogger[] SelectedLoggers
        {
            get => this.selectedLoggers;
            set
            {
                if (this.selectedLoggers == value)
                    return;

                this.selectedLoggers = value;

                this.UpdateLogView();
            }
        }

        private LoggerTree tree;
        private LogsTable logsTable;

        [MenuItem("Tools/Logger/Overview %l")]
        public static void ShowWindow()
        {
            var window = GetWindow<LoggerOverviewEditorWindow>();
            window.titleContent = new GUIContent("Logger Overview");
        }

        private void OnEnable()
        {
            var root = this.rootVisualElement;
            root.style.flexDirection = FlexDirection.Row;
            root.style.paddingLeft = 10;
            root.style.paddingRight = 10;
            root.style.paddingTop = 10;
            root.style.paddingBottom = 10;

            // root.Add(new LoggerTree(loggers =>
            // {
            //     this.SelectedLoggers = loggers;
            // })
            // {
            //     style =
            //     {
            //         width = 300,
            //         marginRight = 10,
            //     },
            // });

            root.Add(new LoggerList(loggers =>
            {
                this.SelectedLoggers = loggers;
            })
            {
                style =
                {
                    width = 300,
                    marginRight = 10,
                },
            });


            this.logsTable = new LogsTable(true)
            {
                style =
                {
                    flexGrow = 1,
                },
            };
            this.logsTable.SetLoggers(this.SelectedLoggers);

            root.Add(this.logsTable);
        }

        private void UpdateLogView()
        {
            this.logsTable.SetLoggers(this.SelectedLoggers);
        }
    }

    public class LoggerTree : VisualElement
    {
        private readonly TreeItemCollection treeItemCollection = new();
        private readonly CustomTreeView treeView;
        private readonly LogsTable logsTable;

        public LoggerTree(Action<IRabbitLogger[]> onSelect)
        {
            this.treeItemCollection.Initialize(LogManager.Instance.Loggers);

            LogManager.OnLoggerRegistered.AddListener(this.OnLoggerAdded);
            LogManager.OnLoggerUnregistered.AddListener(this.OnLoggerRemoved);

            this.treeView = new CustomTreeView();

            this.treeView.SetItems(this.treeItemCollection.GetTree());
            this.treeView.SetOnItemSelected(item =>
            {
                var loggers = new List<IRabbitLogger>();

                this.GetLoggers(item, loggers);

                onSelect(loggers.ToArray());
            });

            this.Add(this.treeView);
        }

        private void GetLoggers(CustomTreeView.TreeViewItem item, List<IRabbitLogger> loggers)
        {
            if (item.Logger != null)
                loggers.Add(item.Logger);

            foreach (var child in item.Children)
            {
                this.GetLoggers(child, loggers);
            }
        }

        private void OnLoggerAdded(IRabbitLogger logger)
        {
            this.treeItemCollection.Add(logger);
            this.Update();
        }

        private void OnLoggerRemoved(IRabbitLogger logger)
        {
            this.treeItemCollection.Remove(logger);
            this.Update();
        }

        public void Update()
        {
            this.treeView.SetItems(this.treeItemCollection.GetTree());
        }
    }

    public class TreeItemCollection
    {
        private int id = 0;
        private Dictionary<IRabbitLogger, CustomTreeView.TreeViewItem> loggers = new();
        private Dictionary<string, CustomTreeView.TreeViewItem> paths = new();
        private Dictionary<int, CustomTreeView.TreeViewItem> items = new();

        public void Initialize(IEnumerable<IRabbitLogger> loggers)
        {
            this.loggers.Clear();
            this.paths.Clear();

            foreach (var logger in loggers)
            {
                this.Add(logger);
            }
        }

        public void Add(IRabbitLogger logger)
        {
            if (this.loggers.ContainsKey(logger))
                return;

            var item = new CustomTreeView.TreeViewItem
            {
                Id = this.GetNextId(),
                Name = logger.Name,
                Logger = logger,
            };

            this.loggers.Add(logger, item);
            this.items.Add(item.Id, item);

            this.GetItem(logger.Path)?.Children.Add(item);
        }

        public void Remove(IRabbitLogger logger)
        {
            this.loggers.Remove(logger, out var item);

            if (item == null)
                return;

            this.items.Remove(item.Id);

            var goItem = this.GetItem(logger.Path);

            goItem.Children.Remove(item);

            if (goItem.Children.Count == 0)
                this.paths.Remove(logger.Path);
        }

        public CustomTreeView.TreeViewItem GetItem(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (this.paths.TryGetValue(path, out var o))
                return o;

            var item = new CustomTreeView.TreeViewItem
            {
                Id = this.GetNextId(),
                Name = path,
                Logger = null,
            };

            this.paths.Add(path, item);
            this.items.Add(item.Id, item);

            return item;
        }

        public CustomTreeView.TreeViewItem[] GetTree()
        {
            var groups = this.loggers.Values.GroupBy(x => x.Logger.Path).Where(x => x.Key != null);

            return groups.Select((group, index) =>
            {
                var item = this.GetItem(group.Key);

                item.Children = group.ToList();

                return item;
            }).OrderBy(x => x.Name).ToArray();

            // return this.loggers.Values.GroupBy(x => x.Logger.Path).Where(x => x.Key != null).Select((group, index) =>
            // {
            //     var item = this.GetItem(group.Key);
            //
            //     var children = group.Select(child =>
            //     {
            //         return new CustomTreeView.TreeViewItem(child.Id, child)
            //         {
            //             Id = child.Id,
            //             Logger = child,
            //             Children = new List<CustomTreeView.TreeViewItem>()
            //         };
            //     });
            //
            //     return new CustomTreeView.TreeViewItem(item.Id, item, children.ToList());
            // }).ToArray();
        }

        private int GetNextId()
        {
            return this.id++;
        }

        // public void Update()
        // {
        //     foreach (var (key, value) in this.items)
        //     {
        //         if (value.IsExpanded)
        //             this.treeView.ExpandItem(value.Id);
        //         else
        //             this.treeView.CollapseItem(value.Id);
        //     }
        // }
    }
}
