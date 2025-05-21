using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera firstPersonCam;
    public CinemachineCamera thirdPersonCam;

    [Header("UI Canvas")]
    public Canvas firstPersonCanvas;

    [Header("Input")]
    public PlayerController playerInput;

    [Header("Settings")]
    public int firstPersonPriority = 20;
    public int thirdPersonPriority = 10;

    private bool inFirstPerson = true;

    void Start()
    {
        ActivateFirstPerson();
    }

    public void ToggleCamera()
    {
        if (inFirstPerson)
            ActivateThirdPerson();
        else
            ActivateFirstPerson();
    }

   public void ActivateFirstPerson()
    {
        thirdPersonCam.Priority = 10;
        firstPersonCam.Priority = 20;

        if (firstPersonCanvas != null)
            firstPersonCanvas.enabled = true;

        if (playerInput != null)
            playerInput.canMove = true;

        inFirstPerson = true;
    }

    public void ActivateThirdPerson()
    {
        Debug.Log("Switching to third person camera");
        thirdPersonCam.Priority = 20;
        firstPersonCam.Priority = 10;

        if (playerInput != null)
            playerInput.canMove = false;

        if (firstPersonCanvas != null)
            firstPersonCanvas.enabled = false;
    }

}
