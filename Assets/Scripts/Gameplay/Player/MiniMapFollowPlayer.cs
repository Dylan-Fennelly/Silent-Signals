using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 50, 0); // Height above player

    void LateUpdate()
    {
        // Follow player position, but keep fixed rotation (looking down)
        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Top-down view
    }
}
