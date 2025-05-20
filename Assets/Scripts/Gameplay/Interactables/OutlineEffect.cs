using UnityEngine;

public class OutlineEffect : MonoBehaviour
    {
        public void SetOutlined(bool isActive)
    {
        // Assuming you have a method to enable/disable the outline effect
        if (isActive)
        {
            Debug.Log("Outline effect enabled");
        }
        else
        {
            // Disable outline effect
        }
    }
}

