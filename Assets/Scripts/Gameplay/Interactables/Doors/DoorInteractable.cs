using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorOpener doorOpener;
    [SerializeField] private string interactableName = "Door";
    [SerializeField] private UnityEvent onInteract;
    public bool isLocked = false;
    public string GetInteractableName()
    {
        return interactableName;
    }

    public void Interact()
    {
        if (isLocked)
        {
            interactableName = "The door is locked.";
            return;
        }
        onInteract?.Invoke();
        doorOpener.ToggleDoor();
    }
    public void Unlock()
    {
        isLocked = false;
        interactableName = "Door";
    }
}
