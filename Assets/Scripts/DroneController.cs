using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float speed = 5f;
    public float pGain = 1.0f;  // Proportional gain
    public float iGain = 0.0f;  // Integral gain
    public float dGain = 0.0f;  // Derivative gain
    public float fixedDeltaTime = 1.0f;  // Time step
    public float maxDistanceFromWall = 3f;

    private Vector2 outputDrone;
    private Vector2 integral;
    private Vector2 previousError;

    private List<Wall> walls;

    private class Wall
    {
        public Vector2 position;
        public string type;
    }

    void Start()
    {
        outputDrone = new Vector2(1.0f, 1.0f);
        integral = Vector2.zero;
        previousError = Vector2.zero;

        // Define the wall positions
        walls = new List<Wall>
        {
            new Wall { position = new Vector2(0, 0), type = "left" },
            new Wall { position = new Vector2(4, 4), type = "right" },
            new Wall { position = new Vector2(2, 2), type = "forward" },
            new Wall { position = new Vector2(1, 0), type = "backward" }
        };
    }

    void Update()
    {
        // Calculate repulsive force
        Vector2 repulsiveForce = CalculateRepulsiveForces(outputDrone, walls);
        Vector2 error = CalculateError(repulsiveForce);

        // Update PID controller
        Vector2 velocity;
        (velocity, previousError, integral) = UpdatePID(integral, previousError, error);

        // Update drone position
        outputDrone += velocity * fixedDeltaTime;
        transform.position = new Vector3(outputDrone.x, transform.position.y, outputDrone.y);

        // Check if we have split in the way
        string moveWay = "forward";
        foreach (var wall in walls)
        {
            if (CheckIfTheDistanceIsMax(outputDrone, wall.position, maxDistanceFromWall))
            {
                if (moveWay != "right") // If there is two way - choose the right
                {
                    moveWay = wall.type;
                }
            }
        }

        Debug.Log($"Drone Position: {outputDrone}, Velocity: {velocity}");
        Debug.Log($"Drone way: {moveWay}");
    }

    bool CheckIfTheDistanceIsMax(Vector2 outputDrone, Vector2 wall, float maxDistanceFromWall)
    {
        Vector2 direction = outputDrone - wall;
        float distance = direction.magnitude;
        return Mathf.Approximately(distance, maxDistanceFromWall);
    }

    Vector2 CalculateRepulsiveForces(Vector2 outputDrone, List<Wall> walls)
    {
        Vector2 totalForce = Vector2.zero;
        foreach (var wall in walls)
        {
            Vector2 direction = outputDrone - wall.position;
            float distance = direction.magnitude;
            if (distance != 0)
            {
                float forceMagnitude = 1 / distance;  // The closer the wall, the stronger the repulsive force
                Vector2 force = (direction / distance) * forceMagnitude;
                totalForce += force;
            }
        }
        return totalForce;
    }

    Vector2 CalculateError(Vector2 repulsiveForce)
    {
        return -repulsiveForce;  // We want to move away from the walls
    }

    (Vector2, Vector2, Vector2) UpdatePID(Vector2 integral, Vector2 previousError, Vector2 error)
    {
        Vector2 proportional = pGain * error;

        // Integral term
        integral += error * fixedDeltaTime;
        Vector2 integralTerm = iGain * integral;

        // Derivative term
        Vector2 derivative = (error - previousError) / fixedDeltaTime;
        Vector2 derivativeTerm = dGain * derivative;

        // Combine terms
        Vector2 pidOutput = proportional + integralTerm + derivativeTerm;

        // Update the previous error
        previousError = error;

        // Calculate velocity
        Vector2 direction = pidOutput.normalized;
        Vector2 velocity = direction * speed;

        return (velocity, previousError, integral);
    }
}