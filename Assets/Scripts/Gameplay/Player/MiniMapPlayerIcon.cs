using UnityEngine;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        // Only rotate around Y, inverse it so icon rotates clockwise as player turns
        Vector3 rotation = transform.localEulerAngles;
        rotation.z = -player.eulerAngles.y;
        transform.localEulerAngles = rotation;
    }
}
