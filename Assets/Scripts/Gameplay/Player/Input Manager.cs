using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    void OnEnable()
    {
        playerControls.Enable();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetPlayerLook()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }
    public bool GetPlayerInteracted()
    {
        return playerControls.Player.Interact.triggered;
    }
}
