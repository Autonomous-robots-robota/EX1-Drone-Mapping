// using UnityEngine;




// public class PID
// {
//     private double Kp;
//     private double Ki;
//     private double Kd;
    
//     private double p_error;
//     private double i_error;
//     private double d_error;

//     public PID() {}

//     ~PID() {}

//     public void Init(double Kp, double Ki, double Kd)
//     {
//         this.Kp = Kp;
//         this.Ki = Ki;
//         this.Kd = Kd;

//         p_error = 0;
//         i_error = 0;
//         d_error = 0;
//     }

//     public void UpdateError(double cte)
//     {
//         d_error = cte - p_error;
//         p_error = cte;
//         i_error += cte;
//     }

//     public double TotalError()
//     {
//         return -Kp * p_error - Ki * i_error - Kd * d_error;
//     }
// }




// public class PlayerRaycast : MonoBehaviour
// {
//     [SerializeField]
//     private float detectionDistance = 120f; // Distance to detect walls
//     [SerializeField]
//     private float emergencyDistance = 30f; // Distance to detect walls
//     [SerializeField]
//     private float rollLeftDistance = 60f; // Distance to detect walls
//     [SerializeField]
//     private float rightDistance = 60f; // Distance to detect walls
//     private LineRenderer lineRenderer;
//     [SerializeField]
//     private LayerMask layerMask;

//     public float speedMove;
//     public float speedRotate;

//     // PID parameters for movement
//     public float moveKp = 1f;
//     public float moveKi = 0.1f;
//     public float moveKd = 0.01f;

//     PID pid_m = new PID();
//     pid_m.Init(moveKp, moveKi, moveKd);

//     // PID parameters for rotation
//     public float rotateKp = 1f;
//     public float rotateKi = 0.1f;
//     public float rotateKd = 0.01f;
	
// 	PID pid_r = new PID();
//     pid_r.Init(rotateKp, rotateKi, rotateKd);

//     //private float moveIntegral = 0f;
//     //private float movePreviousError = 0f;

//     //private float rotateIntegral = 0f;
//     //private float rotatePreviousError = 0f;

//     // Threshold distances
//     public float desiredDistance = 1f; // Desired distance from the walls

//     void Start()
//     {
//         // Initialize the LineRenderer
//         lineRenderer = gameObject.AddComponent<LineRenderer>();
//         lineRenderer.startWidth = 1f;
//         lineRenderer.endWidth = 1f;
//         lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
//         lineRenderer.startColor = Color.green;
//         lineRenderer.endColor = Color.green;
//         lineRenderer.positionCount = 8; // We will draw 4 lines (2 points per line)
//     }

//     void Update()
//     {
//         // Detect walls in four directions relative to the player's orientation
//         float f = DetectAndDrawRay(transform.up, 0);    // Forward
//         float b = DetectAndDrawRay(-transform.up, 1);   // Backward
//         float l = DetectAndDrawRay(-transform.right, 2); // Left
//         float r = DetectAndDrawRay(transform.right, 3);  // Right
//         Debug.Log($"Forward   distance {f}\nBackward  distance {b}\nLeft      distance {l}\nRight     distance {r}");

//         ControlRight(f, b, l, r);

//         //(float move, float rotate) = CalculateMovementAndRotation(f, b, l, r);

//         //control(move, rotate);
//     }

//     void ControlRight(float f, float b, float l, float r)
//     {
//         float moveX, moveY, rotate;
//         rotate = CalcRotation(f,b,r,l);
//         moveX = CalcForward(f);
//         //moveY = CalcRoll(r);

//         transform.Translate(Vector2.up * moveX * speedMove * Time.deltaTime);
//         //transform.Translate(Vector2.right * moveY * speedMove * Time.deltaTime);
//         transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
//     }



//     float CalcRotation(float f, float b, float r float l)
//     {
//         //if (r < emergencyDistance) return 1f;
//         //if (f < rollLeftDistance) return 1f;
//         //if (r > rightDistance) return -1f;
//         //return 0f;
//         //return Mathf.Clamp(f, -1f, 1f);
		
// 		if (r < emergencyDistance && l >= emergencyDistance) { // r is to close to wall and l has plenty of room
// 			pid_r.UpdateError(emergencyDistance - r); 
// 			return pid_r.TotalError();
// 			} 
// 		if (r < emergencyDistance && l < emergencyDistance){ // r and l are close to wall stay in middle
// 			pid_r.UpdateError(r-l); 
// 			return pid_r.TotalError();
		
// 		}
// 		if (f < rollLeftDistance){ // close to wall in front need to turn left
// 			pid_r.UpdateError( rollLeftDistance - f);
// 			return pid_r.TotalError();
// 		}
// 		if (r > rightDistance){ // far from right wall turn towards it
// 			pid_r.UpdateError( rightDistance - r);
// 			return pid_r.TotalError();
// 		}
		
//     }

//     float CalcForward(float f) {
//         if (f < emergencyDistance) return 0f; // EMERGENCY
//         if (f > detectionDistance) return 1f;
//         return Mathf.Clamp(f - emergencyDistance / detectionDistance - emergencyDistance, 0f, 1f);
//     }

//     float CalcRoll(float r)
//     {
//         return Mathf.Clamp(r-25, -1f, 1f);

//     }

//     //void control(float movex, float rotate)
//     //{
//     //    transform.Translate(Vector2.up * movex * speedMove * Time.deltaTime);
//     //    transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
//     //}

//     //(float, float) CalculateMovementAndRotation(float f, float b, float l, float r)
//     //{
//     //    float moveSpeed = 1f;
//     //    float rotateSpeed = 0f;

//     //    //if (f < detectionDistance)
//     //    //{
//     //    //    // Slow down as it gets closer to an obstacle in front and turn right
//     //    //    moveSpeed = Mathf.Clamp(f / detectionDistance, 0.1f, 1f); // Slows down linearly as it approaches the obstacle
//     //    //    rotateSpeed = 1f; // Turn right
//     //    //}
//     //    //else if (r < detectionDistance && l < detectionDistance)
//     //    if (r < detectionDistance && l < detectionDistance)
//     //    {
//     //        // Stay in the middle
//     //        moveSpeed = 1f;
//     //        rotateSpeed = CalcRotate(l, r);
//     //    }
//     //    else
//     //    {
//     //        // Stay at the desired distance away from the right wall
//     //        moveSpeed = 1f;
//     //        rotateSpeed = CalcRotateToMaintainDistance(r);
//     //    }

//     //    return (moveSpeed, rotateSpeed);
//     //}

//     //float CalcRotate(float l, float r)
//     //{
//     //    float error = l - r;
//     //    rotateIntegral += error * Time.deltaTime;
//     //    float derivative = (error - rotatePreviousError) / Time.deltaTime;
//     //    rotatePreviousError = error;

//     //    float output = rotateKp * error + rotateKi * rotateIntegral + rotateKd * derivative;
//     //    return Mathf.Clamp(output, -1f, 1f); // Clamp to [-1, 1] to keep rotation within bounds
//     //}

//     //float CalcRotateToMaintainDistance(float r)
//     //{
//     //    float error = desiredDistance - r; // Error is the difference between desired distance and the right distance
//     //    rotateIntegral += error * Time.deltaTime;
//     //    float derivative = (error - rotatePreviousError) / Time.deltaTime;
//     //    rotatePreviousError = error;

//     //    float output = rotateKp * error + rotateKi * rotateIntegral + rotateKd * derivative;
//     //    return Mathf.Clamp(output, -1f, 1f); // Clamp to [-1, 1] to keep rotation within bounds
//     //}

//     float DetectAndDrawRay(Vector2 direction, int index)
//     {
//         RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, layerMask);
//         float distance = hit.collider != null ? hit.distance : detectionDistance;

//         // Set the positions for the LineRenderer
//         lineRenderer.SetPosition(index * 2, transform.position);
//         lineRenderer.SetPosition(index * 2 + 1, transform.position + (Vector3)direction * distance);

//         return distance;
//     }
// }



using UnityEngine;

public class PID
{
    private float Kp;
    private float Ki;
    private float Kd;

    public float p_error;
    public float i_error;
    public float d_error;

    public float min_range;
    public float max_range;

    public PID() {}

    ~PID() {}

    public void Init(float Kp, float Ki, float Kd, float min_range, float max_range)
    {
        this.Kp = Kp;
        this.Ki = Ki;
        this.Kd = Kd;
        this.min_range = min_range;
        this.max_range = max_range;

        p_error = 0f;
        i_error = 0f;
        d_error = 0f;
    }

    public void UpdateError(float cte)
    {
        d_error = cte - p_error;
        p_error = cte;
        i_error += cte;
    }

    public float TotalError()
    {
        float er = -Kp * p_error - Ki * i_error - Kd * d_error;
        er = er/1200f;
        return Mathf.Clamp( er, min_range, max_range);
    }
}

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    private float detectionDistance = 120f; // Distance to detect walls
    [SerializeField]
    private float emergencyDistance = 30f; // Distance to detect walls
    [SerializeField]
    private float stayMiddle = 120f; // Distance to detect walls
    [SerializeField]
    private float rollLeftDistance = 60f; // Distance to detect walls
    [SerializeField]
    private float rightDistance = 60f; // Distance to detect walls
    private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask layerMask;

    public float speedMove =10f;
    public float speedRotate = 0f;

    // PID parameters for movement
    public float moveKp = 1f;
    public float moveKi = 0f;
    public float moveKd = 0f;

    // PID parameters for rotation
    public float rotateKp = 0f;
    public float rotateKi = 0f;
    public float rotateKd = 0f;

    public PID pid_m;
    public PID pid_r;

    // Threshold distances
    public float desiredDistance = 120f; // Desired distance from the walls

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

        // Initialize PID controllers
        pid_m = new PID();
        pid_m.Init(moveKp, moveKi, moveKd, 0f,1f);

        pid_r = new PID();
        pid_r.Init(rotateKp, rotateKi, rotateKd, -1f,1f);
    }

    void Update()
    {
        // Detect walls in four directions relative to the player's orientation
        float f = DetectAndDrawRay(transform.up, 0);    // Forward
        float b = DetectAndDrawRay(-transform.up, 1);   // Backward
        float l = DetectAndDrawRay(-transform.right, 2); // Left
        float r = DetectAndDrawRay(transform.right, 3);  // Right
        // Debug.Log($"Forward   distance {f}\nBackward  distance {b}\nLeft      distance {l}\nRight     distance {r}");
        // Debug.Log($"before change: \nD_error {pid_r.d_error}\nP_error {pid_r.p_error}\nI_error {pid_r.i_error}");
        ControlRight(f, b, l, r);
        // Debug.Log($"after change: \nD_error {pid_r.d_error}\nP_error {pid_r.p_error}\nI_error {pid_r.i_error}");
    }

    void ControlRight(float f, float b, float l, float r)
    {
        float moveX, rotate;
        rotate = CalcRotation(f, b, r, l);
        moveX = CalcForward(f);
        Debug.Log($"rotate {rotate}, moveX {moveX}");
        transform.Translate(Vector2.up * moveX * speedMove * Time.deltaTime);
        transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
    }

    float CalcRotation(float f, float b, float r, float l)
    {
        if (r < emergencyDistance && l >= stayMiddle)
        { // r is too close to the wall and l has plenty of room
            pid_r.UpdateError(emergencyDistance - r);
            return 0f;
            return pid_r.TotalError();
        }
        else if (r < stayMiddle && l < stayMiddle)
        { // r and l are close to the wall, stay in the middle
            pid_r.UpdateError(r - l);
            return 0f;
            return pid_r.TotalError();
        }
        else if (f < rollLeftDistance)
        { // close to the wall in front, need to turn left
            pid_r.UpdateError(rollLeftDistance - f);
            return 0f;
            return pid_r.TotalError();
        }
        else if (r > rightDistance)
        { // far from the right wall, turn towards it
            pid_r.UpdateError(rightDistance - r);
            return 0f;
            return pid_r.TotalError();
        }

        return 0f; // Default case if none of the conditions are met
    }

    float CalcForward(float f)
    {
        
        if (f < rollLeftDistance){
            pid_m.UpdateError(0f);
            Debug.Log($"too close not moving, distance forward{f},emergency distance{emergencyDistance}");
            return pid_m.TotalError(); 
        }
        if (f > detectionDistance){
            pid_m.UpdateError(detectionDistance);
            Debug.Log($"plenty of room move a lot, distance forward{f},detection distance{detectionDistance}");
            return pid_m.TotalError(); 
        }
        else{
            pid_m.UpdateError((f - emergencyDistance));
            Debug.Log($"getting close slowing down, distance forward{f},emergency distance{emergencyDistance}");
            return pid_m.TotalError(); 
        }
        // return Mathf.Clamp((f - emergencyDistance) / (detectionDistance - emergencyDistance), 0f, 1f);
    }

    // float CalcRoll(float r)
    // {
    //     return Mathf.Clamp(r - 25f, -1f, 1f);
    // }

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


