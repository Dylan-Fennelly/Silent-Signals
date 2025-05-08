using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    [CustomEditor(typeof(LogManagerConfigScriptable))]
    public class LoggerConfigScriptableEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create a container for the custom inspector UI
            var root = new VisualElement();

            // Add the default inspector fields using SerializedObject
            var iterator = this.serializedObject.GetIterator();
            if (iterator.NextVisible(true)) // Skip "m_Script"
            {
                do
                {
                    var propertyField = new PropertyField(iterator);
                    propertyField.Bind(this.serializedObject); // Bind the property field to the serialized object
                    root.Add(propertyField);
                } while (iterator.NextVisible(false));
            }

            // Add a separator
            root.Add(new VisualElement { style = { height = 10 } });

            // Add buttons for each type of ILogTarget
            root.Add(this.CreateLogTargetButtons());

            return root;
        }

        private VisualElement CreateLogTargetButtons()
        {
            var config = (LogManagerConfigScriptable) this.target;

            var addedTypes = config.Targets.Select(x => x?.GetType()).Where(x => x != null).ToList();

            // Find all classes implementing ILogTarget
            var logTargetTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ILogTarget).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass)
                .ToList();

            var nonAddedTypes = logTargetTypes.Where(x => !addedTypes.Contains(x)).ToList();

            var container = new VisualElement();

            if (nonAddedTypes.Count == 0)
                return container;

            container.style.marginTop = 10;

            var header = new Label("Add ILogTarget Types");
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            container.Add(header);

            foreach (var type in nonAddedTypes)
            {
                var button = new Button(() => this.AddLogTarget(type))
                {
                    text = $"Add {type.Name}",
                };
                container.Add(button);
            }

            return container;
        }

        private void AddLogTarget(Type logTargetType)
        {
            var config = (LogManagerConfigScriptable) this.target;

            if (config.Targets.Any(x => x.GetType() == logTargetType))
            {
                Debug.LogWarning($"Log target of type {logTargetType.Name} already exists in the config");
                return;
            }

            // Add an instance of the log target to the scriptable object
            var instance = Activator.CreateInstance(logTargetType) as ILogTarget;

            if (instance != null)
            {
                Undo.RecordObject(config, "Add Log Target");
                // Assuming the LoggerConfigScriptable has a method or property to add log targets
                config.AddTarget(instance);

                EditorUtility.SetDirty(config);

                // Refresh the inspector
                this.Repaint();
            }
            else
            {
                Debug.LogError($"Failed to create an instance of {logTargetType.Name}");
            }
        }
    }
}
