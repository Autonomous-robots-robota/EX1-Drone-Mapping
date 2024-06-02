//using UnityEngine;

//public class PlayerRaycast : MonoBehaviour
//{
//    [SerializeField]
//    private float detectionDistance = 5f; // Distance to detect walls

//    private float drawDistanceL = 5f; 
//    private float drawDistanceR = 5f; 
//    private float drawDistanceU = 5f; 
//    private float drawDistanceD = 5f;

//    void Update()
//    {
//        //drawDistanceL = detectionDistance;
//        //drawDistanceR = detectionDistance;
//        //drawDistanceU = detectionDistance;
//        //drawDistanceD = detectionDistance;
//    // Detect walls in four directions
//        DetectWalls(transform.up);    // Forward
//        DetectWalls(-transform.up);   // Backward
//        DetectWalls(-transform.right); // Left
//        DetectWalls(transform.right);  // Right
//    }

//    void DetectWalls(Vector2 direction)
//    {
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance);

//        if (hit.collider != null)
//        {
//            if(direction == Vector2.down) {
//                drawDistanceD = hit.distance;
//            }
//            else
//            {
//                drawDistanceD = detectionDistance;
//            }
//            if (direction == Vector2.up)
//            {
//                drawDistanceU = hit.distance;
//            }
//            else
//            {
//                drawDistanceU = detectionDistance;
//            }
//            if (direction == Vector2.right)
//            {
//                drawDistanceR = hit.distance;
//            }
//            else
//            {
//                drawDistanceR = detectionDistance;
//            }
//            if (direction == Vector2.left)
//            {
//                drawDistanceL = hit.distance;
//            }
//            else
//            {
//                drawDistanceL = detectionDistance;
//            }

//            // If the ray hits a collider, print the direction and distance to the wall
//            Debug.Log($"Detected wall in direction {direction} at distance {hit.distance}");
//        }
//    }

//    // Optionally, draw the rays in the Scene view for debugging
//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawRay(transform.position, transform.up * drawDistanceU);    // Forward
//        Gizmos.DrawRay(transform.position, -transform.up * drawDistanceD);   // Backward
//        Gizmos.DrawRay(transform.position, -transform.right * drawDistanceL); // Left
//        Gizmos.DrawRay(transform.position, transform.right * drawDistanceR);  // Right
//    }
//}

using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance = 5f; // Distance to detect walls

    private LineRenderer lineRenderer;

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
        DetectAndDrawRay(transform.up, 0);    // Forward
        DetectAndDrawRay(-transform.up, 1);   // Backward
        DetectAndDrawRay(-transform.right, 2); // Left
        DetectAndDrawRay(transform.right, 3);  // Right
    }

    void DetectAndDrawRay(Vector2 direction, int index)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance);
        float distance = hit.collider != null ? hit.distance : detectionDistance;

        // Set the positions for the LineRenderer
        lineRenderer.SetPosition(index * 2, transform.position);
        lineRenderer.SetPosition(index * 2 + 1, transform.position + (Vector3)direction * distance);

        // If the ray hits a collider, print the direction and distance to the wall
        if (hit.collider != null)
        {
            Debug.Log($"Detected wall in direction {direction} at distance {hit.distance}");
        }
    }

    // Optionally, draw the rays in the Scene view for debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * detectionDistance);    // Forward
        Gizmos.DrawRay(transform.position, -transform.up * detectionDistance);   // Backward
        Gizmos.DrawRay(transform.position, -transform.right * detectionDistance); // Left
        Gizmos.DrawRay(transform.position, transform.right * detectionDistance);  // Right
    }
}


