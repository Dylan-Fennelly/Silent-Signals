using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineEffect : MonoBehaviour
{
    private Material materialInstance;


    private void Awake()
    {

        Renderer renderer = GetComponent<Renderer>();
        materialInstance = renderer.sharedMaterial; // Use sharedMaterial if batching is desired
    }

    public void SetOutlined(bool isActive)
    {
        if (materialInstance == null) return;

        materialInstance.SetFloat("_Using_Fresnel", isActive ? 1f : 0f);

        Debug.Log(isActive ? "Outline effect enabled" : "Outline effect disabled");
    }
}
