using UnityEngine;
using DG.Tweening;

public class BuoyReelButton : MonoBehaviour, IInteractable
{
    [Header("Target Buoy System")]
    public HDRPBuoyancySystem targetBuoy;

    [Header("Settings")]
    public string interactName = "Reel In Buoy";

    private Tween reelTween;

    public void Interact()
    {
        if (targetBuoy != null)
        {
            targetBuoy.StartReeling();
            interactName = "Reeling in Buoy";

            // Start rotating around X-axis indefinitely
            reelTween = transform.DOLocalRotate(
                new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }

    public void MarkDone()
    {
        interactName = "";

        // Stop rotation
        if (reelTween != null && reelTween.IsActive())
        {
            reelTween.Kill();
            transform.localRotation = Quaternion.identity; // Optional: Reset rotation
        }
    }

    public string GetInteractableName()
    {
        return interactName;
    }
}
