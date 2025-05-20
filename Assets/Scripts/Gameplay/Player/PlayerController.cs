using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public CinemachineCamera playerCamera;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float interactionDistance = 3f;
    public LayerMask interactableMask;

    [Header("Input")]
    public PlayerControls inputActions; // auto-generated input actions

    private Transform camTransform;
    private float xRotation = 0f;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private IInteractable currentInteractable;
    private OutlineEffect currentOutline;

    private void Awake()
    {
        inputActions = new PlayerControls();

        inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        inputActions.Player.Interact.performed += ctx => TryInteract();

        camTransform = playerCamera.gameObject.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleInteractionRaycast();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.SimpleMove(move * moveSpeed);
    }

    private void HandleMouseLook()
    {
        // Only rotate the player horizontally, let Cinemachine handle the pitch
        float mouseX = lookInput.x * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
    }


    private void HandleInteractionRaycast()
    {
        Ray ray = new Ray(camTransform.position, camTransform.forward);
        RaycastHit hit;
        IInteractable interactable = null;
        OutlineEffect outline = null;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableMask))
        {
            interactable = hit.collider.GetComponent<IInteractable>();
            outline = hit.collider.GetComponent<OutlineEffect>();
        }

        // Handle outline effect
        if (outline != currentOutline)
        {
            if (currentOutline !=null) currentOutline.SetOutlined(false);
            if (outline!= null) outline.SetOutlined(true);
            currentOutline = outline;
        }

        currentInteractable = interactable;
    }

    private void TryInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
