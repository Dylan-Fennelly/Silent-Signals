using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SampleSeismograph : MonoBehaviour, IInteractable
{
    [Header("Target Seismograph")]
    public GameObject Seismograph1;
    public GameObject Seismograph2;

    [Header("Settings")]
    public string interactName = "Collect Seismograph Data";

    [Header("Event")]
    public UnityEvent<DataCollectionManager.MachineType> OnInteractableEvent;

    private Tween seismo1Tween;
    private Tween seismo2Tween;

    public void Interact()
    {
        if (Seismograph1 != null && Seismograph2 != null)
        {
            interactName = "Gathering Data";

            // Start rotating both seismographs around X-axis
            seismo1Tween = Seismograph1.transform.DOLocalRotate(
                new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            seismo2Tween = Seismograph2.transform.DOLocalRotate(
                new Vector3(360, 0, 0), 1f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            StartCoroutine(WaitThenStop());
        }
    }

    public void MarkDone()
    {
        interactName = "";

        // Stop and reset rotation
        if (seismo1Tween != null && seismo1Tween.IsActive())
        {
            seismo1Tween.Kill();
            Seismograph1.transform.localRotation = Quaternion.identity;
        }

        if (seismo2Tween != null && seismo2Tween.IsActive())
        {
            seismo2Tween.Kill();
            Seismograph2.transform.localRotation = Quaternion.identity;
        }

        OnInteractableEvent?.Invoke(DataCollectionManager.MachineType.Seismic);
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
