using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;


public class SnowVFXController : MonoBehaviour
{
    public VisualEffect snowEffect;
    public SnowVFXSettings currentSettings;

    void Start()
    {
        ApplySettings(currentSettings);
    }
    [Button]
    public void ApplySettings(SnowVFXSettings settings)
    {
        snowEffect.SetFloat("Rate of Snow", settings.rateOfSnow);
        snowEffect.SetFloat("LifeTime", settings.lifetime);
        snowEffect.SetFloat("Size", settings.size);
        snowEffect.SetVector3("Gravity/Force", settings.gravity);
        snowEffect.SetFloat("Turbulance - Intensity", settings.turbulenceIntensity);
        snowEffect.SetFloat("Turbulance - Drag", settings.turbulenceDrag);
        snowEffect.SetFloat("Turbulance - Frequency", settings.turbulenceFrequency);
    }

}