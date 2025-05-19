// Assets/Editor/SnowflakeArrayBaker.cs
using UnityEngine;
using UnityEditor;

public class SnowflakeArrayBaker : MonoBehaviour
{
    [Header("Baker Settings")]
    public Material snowflakeMaterial; // your Custom/SnowflakeFractal
    public int resolution = 512;
    public int variants = 8;
    public string outPath = "Assets/SnowflakeArray.asset";

    [ContextMenu("Bake Snowflake Array")]
    void BakeArray()
    {
        if (snowflakeMaterial == null) { Debug.LogError("Assign Material!"); return; }

        var rt = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        var tex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        var arr = new Texture2DArray(resolution, resolution, variants, TextureFormat.RGBA32, false);

        for (int i = 0; i < variants; i++)
        {
            float seed = i / (float)variants;
            snowflakeMaterial.SetFloat("_Seed", seed);

            Graphics.Blit(null, rt, snowflakeMaterial);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            arr.SetPixels(tex.GetPixels(), i);
        }

        AssetDatabase.DeleteAsset(outPath);
        AssetDatabase.CreateAsset(arr, outPath);
        AssetDatabase.SaveAssets();
        Debug.Log($"Baked {variants} snowflakes → {outPath}");
    }
}
