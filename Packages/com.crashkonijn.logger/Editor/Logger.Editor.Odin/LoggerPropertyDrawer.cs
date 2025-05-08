using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace CrashKonijn.Logger.Editor.Odin
{
    public class LoggerPropertyDrawer : OdinValueDrawer<IRabbitLogger>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var logger = this.ValueEntry.SmartValue;

            if (logger == null)
            {
                this.Property.State.Enabled = false;
                return;
            }

            // Logs container
            this.DrawTable(logger);
        }

        private void DrawTable(IRabbitLogger logger)
        {
            if (logger.Logs == null || logger.Logs.Count == 0)
            {
                GUILayout.Label("No logs available.", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            // Header row
            SirenixEditorGUI.BeginHorizontalToolbar();
            GUILayout.Label("Severity", GUILayout.Width(100));
            GUILayout.Label("Message", GUILayout.ExpandWidth(true));
            SirenixEditorGUI.EndHorizontalToolbar();

            // Log rows
            foreach (var log in logger.Logs)
            {
                SirenixEditorGUI.BeginHorizontalToolbar();
                GUILayout.Label(log.severity.ToString(), GUILayout.Width(100));

                // Optional: Color the log message based on severity
                var color = this.GetColor(log.severity);
                GUIHelper.PushColor(color);
                GUILayout.Label(log.message, GUILayout.ExpandWidth(true));
                GUIHelper.PopColor();

                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }

        private Color GetColor(DebugSeverity severity)
        {
            return severity switch
            {
                DebugSeverity.Warning => Color.yellow,
                DebugSeverity.Error => Color.red,
                _ => Color.white,
            };
        }
    }
}
