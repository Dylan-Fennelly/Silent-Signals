using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace CrashKonijn.Logger.Editor
{
    [InitializeOnLoad]
    public class PackageDependencyChecker
    {
        private const string WebSymbol = "RABBIT_LOGGER_WEB";
        private const string Symbol = "RABBIT_LOGGER_1";

        static PackageDependencyChecker()
        {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var buildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
            var currentSymbols = PlayerSettings.GetScriptingDefineSymbols(buildTarget);

            CheckSymbol(buildTarget, currentSymbols);
            CheckWebSymbol(buildTarget, currentSymbols);
        }

        private static void CheckSymbol(NamedBuildTarget buildTarget, string currentSymbols)
        {
            if (currentSymbols.Contains(Symbol))
                return;

            PlayerSettings.SetScriptingDefineSymbols(buildTarget, currentSymbols + ";" + Symbol);
        }

        private static void CheckWebSymbol(NamedBuildTarget buildTarget, string currentSymbols)
        {
            var requiredAssemblies = new string[]
            {
                "endel.nativewebsocket",
                "Newtonsoft.Json",
            };

            var areInStalled = requiredAssemblies.All(IsAssemblyAvailable);

            if (areInStalled && !currentSymbols.Contains(WebSymbol))
            {
                PlayerSettings.SetScriptingDefineSymbols(buildTarget, currentSymbols + ";" + WebSymbol);
                return;
            }

            if (!areInStalled && currentSymbols.Contains(WebSymbol))
            {
                PlayerSettings.SetScriptingDefineSymbols(buildTarget, currentSymbols.Replace(WebSymbol, ""));
            }
        }

        static bool IsAssemblyAvailable(string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetName().Name == assemblyName)
                    return true;
            }

            return false;
        }
    }
}
