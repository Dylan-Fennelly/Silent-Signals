using System.IO;
using UnityEditor;

namespace CrashKonijn.Logger.Editor
{
    public static class LoggerEditorSettings
    {
        public static string BasePath
        {
            get
            {
                var assets = AssetDatabase.FindAssets($"t:Script {nameof(LoggerEditorSettings)}");
                
                return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(assets[0]));
            }
        }
        
        public const string Version = "0.0.1";
        public const string Secret = "203c6fff-8d88-4e21-aa3c-b8718865f8c3";
    }
}