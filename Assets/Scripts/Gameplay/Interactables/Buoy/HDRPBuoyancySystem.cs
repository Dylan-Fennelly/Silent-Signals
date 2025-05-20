using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class HDRPBuoyancySystem : MonoBehaviour
{
    [Header("Water Settings")]
    public WaterSurface targetSurface;

    [Header("Float Points")]
    public Transform[] floatPoints;

    [Header("Buoyancy Settings")]
    public float totalBuoyancyForce = 10f;
    public float dampingFactor = 1.5f;

    [Header("Tether Settings")]
    public Transform anchorPoint;      // Anchor or boat position
    public float tetherStrength = 5f;  // Pull force strength
    public float ropeLength = 5f;      // Max slack
    public float reelSpeed = 2f;       // Units per second when reeling

    [Header("Debug & Visual")]
    public bool debug = false;

    private Rigidbody rb;
    private WaterSearchParameters searchParams = new WaterSearchParameters();
    private WaterSearchResult searchResult = new WaterSearchResult();
    private LineRenderer lineRenderer;
    private bool isReeling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        if (debug)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }
    }

    void FixedUpdate()
    {
        if (targetSurface == null || floatPoints == null || floatPoints.Length == 0) return;

        float perPointBuoyancy = totalBuoyancyForce / floatPoints.Length;

        foreach (var point in floatPoints)
        {
            searchParams.startPositionWS = point.position;
            searchParams.targetPositionWS = point.position;
            searchParams.error = 0.01f;
            searchParams.maxIterations = 8;

            if (targetSurface.ProjectPointOnWaterSurface(searchParams, out searchResult))
            {
                float waterHeight = searchResult.projectedPositionWS.y;
                float depth = waterHeight - point.position.y;

                if (depth > 0)
                {
                    Vector3 springForce = Vector3.up * perPointBuoyancy * depth;
                    float verticalVelocity = rb.GetPointVelocity(point.position).y;
                    Vector3 dampingForce = Vector3.down * dampingFactor * verticalVelocity;

                    rb.AddForceAtPosition(springForce + dampingForce, point.position, ForceMode.Force);
                }
            }
        }

        ApplyTetherForce();
        ApplyUprightTorque();

    }

    void LateUpdate()
    {
        if (lineRenderer != null && anchorPoint != null)
        {
            lineRenderer.SetPosition(0, anchorPoint.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    void ApplyTetherForce()
    {
        if (anchorPoint == null) return;

        Vector3 toAnchor = anchorPoint.position - transform.position;
        float currentDistance = toAnchor.magnitude;

        if (isReeling || currentDistance > ropeLength)
        {
            Vector3 pullDir = toAnchor.normalized;
            float pullStrength = isReeling ? tetherStrength * 2f : tetherStrength;
            Vector3 velocity = rb.linearVelocity;
            Vector3 horizontalToAnchor = Vector3.ProjectOnPlane(toAnchor, Vector3.up);

            float currentSpeed = Vector3.Dot(velocity, pullDir);
            float maxSpeed = 2f; // Set your desired max reel speed

            if (currentSpeed < maxSpeed)
            {
                rb.AddForce(pullDir * pullStrength, ForceMode.Force);
            }


            if (isReeling)
            {
                ropeLength = Mathf.Max(0f, ropeLength - reelSpeed * Time.fixedDeltaTime);
            }
        }
    }

    void ApplyUprightTorque()
    {
        // World up
        Vector3 currentUp = transform.up;
        Vector3 torqueDirection = Vector3.Cross(currentUp, Vector3.up); // How far off from upright

        float angleOffUp = torqueDirection.magnitude;

        // Apply torque only if tilted significantly
        if (angleOffUp > 0.001f)
        {
            // Strength can be scaled based on angle and a coefficient
            float uprightTorqueStrength = 10f; // Tune this
            Vector3 correctiveTorque = torqueDirection.normalized * uprightTorqueStrength * angleOffUp;

            rb.AddTorque(correctiveTorque - rb.angularVelocity * 0.5f, ForceMode.Force); // Add damping
        }
    }


    // Call this to begin reeling in the buoy
    public void StartReeling()
    {
        isReeling = true;
    }

    // Optional: stop reeling when needed
    public void StopReeling()
    {
        isReeling = false;
    }
}
