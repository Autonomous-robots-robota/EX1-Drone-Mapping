using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance = 70f; // Distance to detect walls
    [SerializeField]
    private float emergencyDistance = 5f; // Distance to detect walls
    [SerializeField]
    private float rollLeftDistance = 60f;
    //[SerializeField]
    //private float stayMiddle = 15f; // Distance to detect walls
    [SerializeField]
    private float rightDistance = 20f; // Distance to detect walls
    private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField] 
    PIDController rotateController;

    [SerializeField] private int tps;

    public float speedMove;
    public float speedRotate;


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

        StartCoroutine(CallFunctionRepeatedly());
    }

    IEnumerator CallFunctionRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f / tps); // Adjust to call the function 20 times per second
            UpdateParams();
        }
    }

    void UpdateParams()
    {
        // Detect walls in four directions relative to the player's orientation
        float f = DetectAndDrawRay(transform.up, 0);    // Forward
        float b = DetectAndDrawRay(-transform.up, 1);   // Backward
        float l = DetectAndDrawRay(-transform.right, 2); // Left
        float r = DetectAndDrawRay(transform.right, 3);  // Right
        Debug.Log($"Forward   distance {f}\nBackward  distance {b}\nLeft      distance {l}\nRight     distance {r}");

        ControlRight(f, b, l, r);
    }

    void Update()
    {
        //// Detect walls in four directions relative to the player's orientation
        //float f = DetectAndDrawRay(transform.up, 0);    // Forward
        //float b = DetectAndDrawRay(-transform.up, 1);   // Backward
        //float l = DetectAndDrawRay(-transform.right, 2); // Left
        //float r = DetectAndDrawRay(transform.right, 3);  // Right
        //Debug.Log($"Forward   distance {f}\nBackward  distance {b}\nLeft      distance {l}\nRight     distance {r}");

        //ControlRight(f, b, l, r);

    }

    void ControlRight(float f, float b, float l, float r)
    {
        float moveX, moveY, rotate;
        rotate = CalcRotation(f,r, l);
        moveX = CalcForward(f);
        //moveY = CalcRoll(r);

        transform.Translate(Vector2.up * moveX * speedMove * Time.deltaTime);
        //transform.Translate(Vector2.right * moveY * speedMove * Time.deltaTime);
        transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
    }

    float CalcRotation(float f, float r, float l)
    {
    //    if (r < emergencyDistance)
    //    { // r is too close to the wall and l has plenty of room
    //        return rotateController.UpdateAngle(tps, r, rightDistance);
    //    }
        if (f < rollLeftDistance)
        { // close to the wall in front, need to turn left
            return 1;
            return rotateController.UpdateAngle(tps, f, rollLeftDistance);
        }
        else if(r < rightDistance && l < rightDistance)
        { // r and l are close to the wall, stay in the middle
            float closer = Mathf.Min(r, l);
            //return rotateController.UpdateAngle(1.0f / tps, closer, stayMiddle * Mathf.Sign(r-l));
            return rotateController.UpdateAngle(tps, r, (r+l)/2);
        }
        //else if (r > rightDistance)
        //{ // far from the right wall, turn towards it
            return rotateController.UpdateAngle(tps, r, rightDistance);
        //}
        //return 0f;
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