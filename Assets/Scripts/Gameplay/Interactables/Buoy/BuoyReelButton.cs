using UnityEngine;

public class BuoyReelButton : MonoBehaviour, IInteractable
{
    [Header("Target Buoy System")]
    public HDRPBuoyancySystem targetBuoy;

    [Header("Settings")]
    public string interactName = "Reel In Buoy";

    public void Interact()
    {
        if (targetBuoy != null)
        {
            targetBuoy.StartReeling();
            interactName = "Button Inactive";

            Debug.Log("Reeling in buoy...");
        }
    }

    public string GetInteractableName()
    {
        return interactName;
    }
}
