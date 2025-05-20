using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorOpener doorOpener;
    [SerializeField] private string interactableName = "Door";
    [SerializeField] private UnityEvent onInteract;
    public string GetInteractableName()
    {
        return interactableName;
    }

    public void Interact()
    {
        onInteract?.Invoke();
        doorOpener.ToggleDoor();
    }
}
