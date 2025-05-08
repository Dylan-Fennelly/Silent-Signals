using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class CustomTreeView : VisualElement
    {
        public class TreeViewItem
        {
            public int Id;
            public string Name;
            public List<TreeViewItem> Children = new();
            public bool IsExpanded;
            public IRabbitLogger Logger;
        }

        private readonly List<TreeViewItem> _items = new();
        private Action<TreeViewItem> _onItemSelected;
        private int selectedItemId = -1;

        public CustomTreeView()
        {
            this.style.flexDirection = FlexDirection.Column;
        }

        public void SetItems(TreeViewItem[] items)
        {
            this._items.Clear();
            this._items.AddRange(items);
            this.Refresh();
        }

        public void SetOnItemSelected(Action<TreeViewItem> onItemSelected)
        {
            this._onItemSelected = onItemSelected;
        }

        private void Refresh()
        {
            this.Clear();
            foreach (var item in this._items)
            {
                this.AddItem(item, 0);
            }
        }

        private void AddItem(TreeViewItem item, int depth)
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginLeft = depth * 16,
                },
            };

            if (this.selectedItemId == item.Id)
                container.style.backgroundColor = new StyleColor(Color.gray);

            if (item.Children.Count > 0)
            {
                var toggle = new Foldout() { value = item.IsExpanded };
                toggle.RegisterValueChangedCallback(evt =>
                {
                    item.IsExpanded = evt.newValue;
                    this.Refresh();
                });
                container.Add(toggle);
            }

            var label = new Label(item.Name);
            label.RegisterCallback<ClickEvent>(_ => this.OnItemClick(item));
            label.style.marginLeft = item.Children.Count > 0 ? 1 : 25;
            container.Add(label);

            this.Add(container);

            if (item.IsExpanded)
            {
                foreach (var child in item.Children)
                {
                    this.AddItem(child, depth + 1);
                }
            }
        }

        private void OnItemClick(TreeViewItem item)
        {
            this.selectedItemId = item.Id;
            this._onItemSelected?.Invoke(item);

            this.Refresh();
        }

        public new class UxmlFactory : UxmlFactory<CustomTreeView, UxmlTraits> { }
    }
}
