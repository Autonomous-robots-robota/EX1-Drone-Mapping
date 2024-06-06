using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance = 70f; // Distance to detect walls
    [SerializeField]
    private float emergencyDistance = 5f; // Distance to detect walls
    [SerializeField]
    private float rollLeftDistance = 15f; // Distance to detect walls
    [SerializeField]
    private float rightDistance = 20f; // Distance to detect walls
    private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask layerMask;

    public float speedMove;
    public float speedRotate;

    //// PID parameters for movement
    //public float moveKp = 1f;
    //public float moveKi = 0.1f;
    //public float moveKd = 0.01f;

    //// PID parameters for rotation
    //public float rotateKp = 1f;
    //public float rotateKi = 0.1f;
    //public float rotateKd = 0.01f;

    //private float moveIntegral = 0f;
    //private float movePreviousError = 0f;

    //private float rotateIntegral = 0f;
    //private float rotatePreviousError = 0f;

    // Threshold distances
    public float desiredDistance = 1f; // Desired distance from the walls

    void Start()
    {
        // Initialize the LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.positionCount = 8; // We will draw 4 lines (2 points per line)
    }

    void Update()
    {
        // Detect walls in four directions relative to the player's orientation
        float f = DetectAndDrawRay(transform.up, 0);    // Forward
        float b = DetectAndDrawRay(-transform.up, 1);   // Backward
        float l = DetectAndDrawRay(-transform.right, 2); // Left
        float r = DetectAndDrawRay(transform.right, 3);  // Right
        Debug.Log($"Forward   distance {f}\nBackward  distance {b}\nLeft      distance {l}\nRight     distance {r}");

        ControlRight(f, b, l, r);

        //(float move, float rotate) = CalculateMovementAndRotation(f, b, l, r);

        //control(move, rotate);
    }

    void ControlRight(float f, float b, float l, float r)
    {
        float moveX, moveY, rotate;
        rotate = CalcRotation(f,r);
        moveX = CalcForward(f);
        //moveY = CalcRoll(r);

        transform.Translate(Vector2.up * moveX * speedMove * Time.deltaTime);
        //transform.Translate(Vector2.right * moveY * speedMove * Time.deltaTime);
        transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
    }



    float CalcRotation(float f, float r)
    {
        if (r < emergencyDistance) return 1f;
        if (f < rollLeftDistance) return 1f;
        if (r > rightDistance) return -1f;
        return 0f;
        //return Mathf.Clamp(f, -1f, 1f);
    }

    float CalcForward(float f) {
        if (f < emergencyDistance) return 0f; // EMERGENCY
        if (f > detectionDistance) return 1f;
        return Mathf.Clamp(f - emergencyDistance / detectionDistance - emergencyDistance, 0f, 1f);
    }

    float CalcRoll(float r)
    {
        return Mathf.Clamp(r-25, -1f, 1f);

    }

    //void control(float movex, float rotate)
    //{
    //    transform.Translate(Vector2.up * movex * speedMove * Time.deltaTime);
    //    transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
    //}

    //(float, float) CalculateMovementAndRotation(float f, float b, float l, float r)
    //{
    //    float moveSpeed = 1f;
    //    float rotateSpeed = 0f;

    //    //if (f < detectionDistance)
    //    //{
    //    //    // Slow down as it gets closer to an obstacle in front and turn right
    //    //    moveSpeed = Mathf.Clamp(f / detectionDistance, 0.1f, 1f); // Slows down linearly as it approaches the obstacle
    //    //    rotateSpeed = 1f; // Turn right
    //    //}
    //    //else if (r < detectionDistance && l < detectionDistance)
    //    if (r < detectionDistance && l < detectionDistance)
    //    {
    //        // Stay in the middle
    //        moveSpeed = 1f;
    //        rotateSpeed = CalcRotate(l, r);
    //    }
    //    else
    //    {
    //        // Stay at the desired distance away from the right wall
    //        moveSpeed = 1f;
    //        rotateSpeed = CalcRotateToMaintainDistance(r);
    //    }

    //    return (moveSpeed, rotateSpeed);
    //}

    //float CalcRotate(float l, float r)
    //{
    //    float error = l - r;
    //    rotateIntegral += error * Time.deltaTime;
    //    float derivative = (error - rotatePreviousError) / Time.deltaTime;
    //    rotatePreviousError = error;

    //    float output = rotateKp * error + rotateKi * rotateIntegral + rotateKd * derivative;
    //    return Mathf.Clamp(output, -1f, 1f); // Clamp to [-1, 1] to keep rotation within bounds
    //}

    //float CalcRotateToMaintainDistance(float r)
    //{
    //    float error = desiredDistance - r; // Error is the difference between desired distance and the right distance
    //    rotateIntegral += error * Time.deltaTime;
    //    float derivative = (error - rotatePreviousError) / Time.deltaTime;
    //    rotatePreviousError = error;

    //    float output = rotateKp * error + rotateKi * rotateIntegral + rotateKd * derivative;
    //    return Mathf.Clamp(output, -1f, 1f); // Clamp to [-1, 1] to keep rotation within bounds
    //}

    float DetectAndDrawRay(Vector2 direction, int index)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, layerMask);
        float distance = hit.collider != null ? hit.distance : detectionDistance;

        // Set the positions for the LineRenderer
        lineRenderer.SetPosition(index * 2, transform.position);
        lineRenderer.SetPosition(index * 2 + 1, transform.position + (Vector3)direction * distance);

        return distance;
    }
}