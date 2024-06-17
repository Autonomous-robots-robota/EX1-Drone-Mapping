using System.Collections;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField] private float detectionDistance = 120f;
    [SerializeField] private float emergencyDistance = 20f;
    [SerializeField] private float frontDistance = 25f;
    [SerializeField] private float sideDistance = 30f;
    [SerializeField] private float tunnelDistance = 20f;

    [Header("Speed Settings")]
    [SerializeField] private float speedMove;
    [SerializeField] private float speedRotate;

    [Header("Controller Settings")]
    [SerializeField] private PIDController rotateController;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int fps = 60;
    [SerializeField] private int sensPS = 20;

    [Header("Acceleration Settings")]
    [SerializeField] private float maxAcceleration = 40f;

    private LineRenderer lineRenderer;
    [SerializeField] private float f, b, r, l;
    [SerializeField] private bool isR;
    [SerializeField] private bool isInTunnel;
    private Vector2 currentVelocity = Vector2.zero;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(CallFunctionRepeatedly());
    }

    IEnumerator CallFunctionRepeatedly()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f / fps);
            //DrawLines();
            ControlMovement();
            i++;
            if (i % (fps / sensPS) == 0) { 
                UpdateSensors();
                DrawLines();
            }
            if(i % (5f * fps) == 0 && isInTunnel) { 
                isR = !isR;
            }
        }
    }

    private void ControlMovement()
    {
        float moveX = CalcForward(f);
        float rotate = CalcRotation(f, r, l);

        Vector2 desiredVelocity = Vector2.up * moveX * speedMove;
        Vector2 acceleration = (desiredVelocity - currentVelocity) / Time.deltaTime;

        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        currentVelocity += acceleration * Time.deltaTime;

        //transform.Translate(currentVelocity * Time.deltaTime);

        transform.Translate(Vector2.up * moveX * speedMove * Time.deltaTime);
        transform.Rotate(0f, 0f, rotate * speedRotate * Time.deltaTime, Space.Self);
    }

    private void DrawLines()
    {
        Vector3[] positions = new Vector3[]
        {
            transform.position + transform.up * f,
            transform.position - transform.up * b,
            transform.position, 
            transform.position - transform.right * l,
            transform.position + transform.right * r
        };
        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }

    private void UpdateSensors()
    {
        f = GetSensorData(transform.up);
        b = GetSensorData(-transform.up);
        l = GetSensorData(-transform.right);
        r = GetSensorData(transform.right);

        if (r < tunnelDistance && l < tunnelDistance)
        {
            isInTunnel = true;
        }
        else if ((r == detectionDistance || l == detectionDistance) && isInTunnel)
        {
            isR = !isR;
            isInTunnel = false;
            //transform.Rotate(0, 0, isR && r == detectionDistance ? -90 : 90);
        }
        if (r < emergencyDistance && l < emergencyDistance && f < emergencyDistance) transform.Rotate(0, 0, 180);
    }

    private float GetSensorData(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, layerMask);
        return hit.collider != null ? hit.distance * Random.Range(0.98f, 1.02f) : detectionDistance;
    }

    private float CalcRotation(float f, float r, float l)
    {
        if (f < frontDistance) return isR ? 1 : -1;
        return r < tunnelDistance && l < tunnelDistance
            ? rotateController.UpdateAngle(fps, r, (r + l) / 2)
            : (isR ? 1 : -1) * rotateController.UpdateAngle(fps, isR ? r : l, sideDistance);
    }

    private float CalcForward(float f)
    {
        return f < emergencyDistance ? 0f : Mathf.Clamp((f - emergencyDistance) / (detectionDistance - emergencyDistance), 0f, 1f);
    }
}
