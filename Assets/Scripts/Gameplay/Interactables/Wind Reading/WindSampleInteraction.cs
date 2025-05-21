using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WindSampleInteraction : MonoBehaviour,IInteractable
{

    [Header("Target Wind Sensor")]
    public GameObject WindSensor;
    [SerializeField] private SnowVFXController SnowVFXController;
    [SerializeField] private SnowVFXSettings SnowVFXSettings;
    [Header("Settings")]
    public string interactName = "Collect Wind Data";

    private Tween reelTween;

    [Header("Event")]
    public UnityEvent<DataCollectionManager.MachineType> OnInteractableEvent;
    public void Interact()
    {
        if (WindSensor != null&&SnowVFXController !=null)
        {
            interactName = "Gathering Data";

            // Start rotating around X-axis indefinitely
            reelTween = WindSensor.transform.DOLocalRotate(
                new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
            SnowVFXController.ApplySettings(SnowVFXSettings);
            StartCoroutine(WaitThenStop());
        }
    }

    public void MarkDone()
    {
        interactName = "";

        // Stop rotation
        if (reelTween != null && reelTween.IsActive())
        {
            reelTween.Kill();
            WindSensor.transform.localRotation = Quaternion.identity;
        }
        OnInteractableEvent?.Invoke(DataCollectionManager.MachineType.WindData);
    }

    public string GetInteractableName()
    {
        return interactName;
    }
    public IEnumerator WaitThenStop()
    {
        yield return new WaitForSeconds(5f);
        MarkDone();
    }
}

