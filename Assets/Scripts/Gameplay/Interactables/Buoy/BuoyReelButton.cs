using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class BuoyReelButton : MonoBehaviour, IInteractable
{
    [Header("Target Buoy System")]
    public HDRPBuoyancySystem targetBuoy;

    [SerializeField] private VisualEffect smokeEffect;
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

            if(smokeEffect != null)
            {
                smokeEffect.enabled = true; // Enable the smoke effect
                smokeEffect.Play();
            }
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

        if (smokeEffect != null)
        {
            smokeEffect.Stop();     // Stops emission
            smokeEffect.enabled = false; // Fully disables it (optional depending on effect type)
        }
    }

    public string GetInteractableName()
    {
        return interactName;
    }
}
