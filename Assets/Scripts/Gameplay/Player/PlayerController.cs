using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController characterController;
    public CinemachineCamera playerCamera;
    public Animator playerAnimator;
    public TextMeshProUGUI interactableText;
    private GameObject lastOutlinedObject;


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

        // Calculate movement velocity on the horizontal plane
        float horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

        // Toggle walking animation
        bool isWalking = horizontalVelocity > 0.1f;
        playerAnimator.SetBool("isMoving", isWalking);
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

            // Show interactable name only if different
            if (interactable != null)
            {
                string name = interactable.GetInteractableName();

                if (!interactableText.gameObject.activeSelf)
                    interactableText.gameObject.SetActive(true);

                if (interactableText.text != name)
                    interactableText.text = name;
            }

            // Outline only if the object changed
            if (hit.collider.gameObject != lastOutlinedObject)
            {
                if (currentOutline != null)
                    currentOutline.SetOutlined(false);

                outline = hit.collider.GetComponent<OutlineEffect>();
                if (outline != null)
                    outline.SetOutlined(true);

                currentOutline = outline;
                lastOutlinedObject = hit.collider.gameObject;
            }

            currentInteractable = interactable;
        }
        else
        {
            // Clear UI and outline
            interactableText.gameObject.SetActive(false);

            if (currentOutline != null)
            {
                currentOutline.SetOutlined(false);
                currentOutline = null;
                lastOutlinedObject = null;
            }

            currentInteractable = null;
        }
    }



    private void TryInteract()
    {
        //playerAnimator.SetTrigger("grab");
        if (currentInteractable != null)
        {
            playerAnimator.SetTrigger("grab");
            currentInteractable.Interact();
            interactableText.gameObject.SetActive(false);
        }
    }
}
