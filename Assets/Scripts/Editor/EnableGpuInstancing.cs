using UnityEngine;
using UnityEditor;
using System.IO;

public class EnableGPUInstancing : EditorWindow
{
    [MenuItem("Tools/Enable GPU Instancing on All Materials")]
    public static void EnableInstancingOnAllMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int changedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.shader != null)
            {
                // Only change if not already enabled
                if (!mat.enableInstancing)
                {
                    mat.enableInstancing = true;
                    EditorUtility.SetDirty(mat); // Mark for saving
                    changedCount++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"GPU Instancing enabled on {changedCount} material(s).");
    }
}
