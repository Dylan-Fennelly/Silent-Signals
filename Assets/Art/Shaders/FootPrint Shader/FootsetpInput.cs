using UnityEngine;

public class FootstepInput : MonoBehaviour
{
    public FootprintPainter painter;
    public KeyCode triggerKey = KeyCode.Space;
    public float maxRayDistance = 2f;
    public LayerMask groundLayers;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            Ray ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundLayers))
            {
                painter.LeaveFootprint(hit.point);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1f);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.red, 1f);
            }
        }
    }
}
