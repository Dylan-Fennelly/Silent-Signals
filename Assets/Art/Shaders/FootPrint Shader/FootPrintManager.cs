using UnityEngine;

public class FootprintManager : MonoBehaviour
{
    public Material footprintMaterial;
    public RenderTexture footprintMask;
    public Vector2 worldOrigin = Vector2.zero;
    public Vector2 worldSize = new Vector2(100, 100); // Must match your Decal Projector area

    void Start()
    {
       // ClearFootprintMask();
        // Assign shader values
        footprintMaterial.SetTexture("_FootprintMask", footprintMask);
        footprintMaterial.SetVector("_WorldOffset", worldOrigin);
        footprintMaterial.SetVector("_WorldSize", worldSize);

        // Clear the mask so the whole area starts as "untouched snow"

    }

    void ClearFootprintMask()
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = footprintMask;
        GL.Clear(true, true, Color.white); // white = clean snow
        RenderTexture.active = active;
    }
}
