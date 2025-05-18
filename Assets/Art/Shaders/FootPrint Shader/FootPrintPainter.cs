using UnityEngine;

public class FootprintPainter : MonoBehaviour
{
    public RenderTexture footprintMask;
    public Texture2D footprintStamp;
    public Vector2 worldOrigin = Vector2.zero;
    public Vector2 worldSize = new Vector2(100, 100); // Match decal area

    public int footprintSizePixels = 16; // size of stamp in texture space

    public void LeaveFootprint(Vector3 worldPos)
    {
        // Convert world space position to 0-1 UV
        float u = (worldPos.x - worldOrigin.x) / worldSize.x;
        float v = (worldPos.z - worldOrigin.y) / worldSize.y;

        // Ignore if out of bounds
        if (u < 0 || u > 1 || v < 0 || v > 1)
            return;

        int px = (int)(u * footprintMask.width);
        int py = (int)((1-v) * footprintMask.height);

        RenderTexture.active = footprintMask;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, footprintMask.width, footprintMask.height, 0);

        // Draw the footprint stamp texture onto the mask
        Graphics.DrawTexture(
            new Rect(px - footprintSizePixels / 2, py - footprintSizePixels / 2, footprintSizePixels, footprintSizePixels),
            footprintStamp
        );

        GL.PopMatrix();
        RenderTexture.active = null;
    }
}
