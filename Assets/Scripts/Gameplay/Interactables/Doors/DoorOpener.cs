using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public float openAngle = 90f;
    public float duration = 1f;

    [EnumToggleButtons]
    public Axis rotationAxis = Axis.Y; // Select axis of rotation

    private bool isOpen = false;
    private Quaternion initialRotation;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    [Button]
    public void ToggleDoor()
    {
        if (isOpen)
        {
            transform.DOLocalRotateQuaternion(initialRotation, duration);
        }
        else
        {
            Vector3 axisDelta = Vector3.zero;

            switch (rotationAxis)
            {
                case Axis.X:
                    axisDelta = new Vector3(openAngle, 0, 0);
                    break;
                case Axis.Y:
                    axisDelta = new Vector3(0, openAngle, 0);
                    break;
                case Axis.Z:
                    axisDelta = new Vector3(0, 0, openAngle);
                    break;
            }

            transform.DOLocalRotate(transform.localEulerAngles + axisDelta, duration, RotateMode.FastBeyond360);
        }

        isOpen = !isOpen;
    }
}
