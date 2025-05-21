using UnityEngine;

[CreateAssetMenu(fileName = "SnowVFXSettings", menuName = "VFX/Snow Settings")]
public class SnowVFXSettings : ScriptableObject
{
    [Header("Snow Parameters")]
    public float rateOfSnow = 10000f;
    public float lifetime = 6f;
    public float size = 0.152f;

    [Header("Gravity / Force")]
    public Vector3 gravity = new Vector3(0, -9.81f, 0);

    [Header("Turbulence")]
    public float turbulenceIntensity = 7.89f;
    public float turbulenceDrag = 2f;
    public float turbulenceFrequency = 7.35f;
}
