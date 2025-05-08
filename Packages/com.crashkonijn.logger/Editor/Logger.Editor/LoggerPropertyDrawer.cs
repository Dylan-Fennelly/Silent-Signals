using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    [CustomPropertyDrawer(typeof(IRabbitLogger), true)]
    public class LoggerPropertyDrawer : PropertyDrawer
    {
        private IRabbitLogger logger;
        private ListView logsView;

        private IRabbitLogger Logger
        {
            get => this.logger;
            set
            {
                if (this.logger == value)
                    return;

                if (this.logger != null)
                    this.logger.OnLog.RemoveListener(this.OnLog);

                this.logger = value;

                this.logger.OnLog.AddListener(this.OnLog);
            }
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var foldout = new Foldout
            {
                text = property.displayName, // Display the property name
                value = true,                // Default to expanded
            };

            if (!Application.isPlaying)
                return new VisualElement();

            // Create property container element.
            var container = new VisualElement()
            {
                style =
                {
                    borderTopColor = Color.black,
                    borderTopWidth = 1,
                    borderBottomColor = Color.black,
                    borderBottomWidth = 1,
                    borderLeftColor = Color.black,
                    borderLeftWidth = 1,
                    borderRightColor = Color.black,
                    borderRightWidth = 1,
                    borderBottomLeftRadius = 5,
                    borderBottomRightRadius = 5,
                    borderTopLeftRadius = 5,
                    borderTopRightRadius = 5,
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 5,
                    paddingRight = 5,
                    marginBottom = 5,
                    marginTop = 5,
                },
            };

            if (property.managedReferenceValue is not IRabbitLogger instance)
                return container;

            this.Logger = instance;

            this.logsView = new ListView(instance.Logs, 20, () => new Label(), (element, index) =>
            {
                var label = (element as Label);
                label.text = instance.Logs[index].ToString();
            });

            container.Add(this.logsView);

            foldout.Add(container);

            return foldout;
        }

        // Returns a GUIStyle that mimics a card using the help box style.
        private GUIStyle GetCardStyle()
        {
            var cardStyle = new GUIStyle(EditorStyles.helpBox)
            {
                richText = true,
                // Padding gives the appearance of inner margins.
                padding = new RectOffset(5, 5, 5, 5),
            };
            return cardStyle;
        }

        // Returns a GUIStyle for drawing individual log messages.
        private GUIStyle GetLogStyle()
        {
            var logStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true,
            };
            return logStyle;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);
            // return;

            EditorGUI.BeginProperty(position, label, property);

            // Retrieve the IRabbitLogger instance.
            var logger = this.fieldInfo.GetValue(property.serializedObject.targetObject) as IRabbitLogger;
            if (logger == null)
            {
                var invalidText = EditorApplication.isPlaying ? "Logger is null or invalid" : "Logger is only available in play mode";

                EditorGUI.LabelField(position, label.text, invalidText);
                EditorGUI.EndProperty();
                return;
            }

            this.Logger = logger;

            // Draw the foldout header.
            var headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                var cardStyle = this.GetCardStyle();
                var logStyle = this.GetLogStyle();

                // Calculate the total height needed for the content inside the card.
                var spacing = EditorGUIUtility.standardVerticalSpacing;
                var cardContentHeight = 0f;
                for (var i = 0; i < logger.Logs.Count; i++)
                {
                    var log = logger.Logs[i];
                    var content = new GUIContent(log.message);
                    // The available width for log text is reduced by the card's horizontal padding.
                    var logHeight = logStyle.CalcHeight(content, position.width - cardStyle.padding.horizontal);
                    cardContentHeight += logHeight;
                    if (i < logger.Logs.Count - 1)
                    {
                        cardContentHeight += spacing;
                    }
                }

                // The total card height includes the content plus the card's vertical padding.
                var cardHeight = cardContentHeight + cardStyle.padding.vertical;

                // Position the card below the header.
                var yOffset = position.y + EditorGUIUtility.singleLineHeight + spacing;
                var cardRect = new Rect(position.x, yOffset, position.width, cardHeight);

                // Draw the card background.
                GUI.Box(cardRect, GUIContent.none, cardStyle);

                // Draw each log inside the card.
                var innerY = cardRect.y + cardStyle.padding.top;
                foreach (var log in logger.Logs)
                {
                    var content = new GUIContent(log.ToString());
                    var logHeight = logStyle.CalcHeight(content, cardRect.width - cardStyle.padding.horizontal);
                    var logRect = new Rect(cardRect.x + cardStyle.padding.left, innerY, cardRect.width - cardStyle.padding.horizontal, logHeight);
                    EditorGUI.LabelField(logRect, content, logStyle);
                    innerY += logHeight + spacing;
                }
            }

            EditorGUI.EndProperty();

            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Retrieve the IRabbitLogger instance.
            var logger = this.fieldInfo.GetValue(property.serializedObject.targetObject) as IRabbitLogger;
            if (logger == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            // Start with the header height.
            var totalHeight = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                totalHeight += EditorGUIUtility.standardVerticalSpacing;

                var cardStyle = this.GetCardStyle();
                var logStyle = this.GetLogStyle();
                var spacing = EditorGUIUtility.standardVerticalSpacing;
                var cardContentHeight = 0f;

                // In GetPropertyHeight, we don't have the actual rect width.
                // We'll use the current view width as an approximation.
                var availableWidth = EditorGUIUtility.currentViewWidth > 0 ? EditorGUIUtility.currentViewWidth : 400f;

                for (var i = 0; i < logger.Logs.Count; i++)
                {
                    var log = logger.Logs[i];
                    var content = new GUIContent(log.message);
                    var logHeight = logStyle.CalcHeight(content, availableWidth - cardStyle.padding.horizontal);
                    cardContentHeight += logHeight;
                    if (i < logger.Logs.Count - 1)
                    {
                        cardContentHeight += spacing;
                    }
                }

                var cardHeight = cardContentHeight + cardStyle.padding.vertical;
                totalHeight += cardHeight;
            }

            return totalHeight;
        }

        private void OnLog(Log loger)
        {
            this.logsView?.RefreshItems();
        }
    }
}
