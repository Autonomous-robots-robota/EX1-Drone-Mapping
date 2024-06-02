using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    private Rigidbody2D rb;
    private Vector2 movement;
    //private float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Get input for rotation
        //rotation = Input.GetAxis("Rotate");

        // Set movement vector
        movement = new Vector2(moveX, moveY);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Rotate the player
        //if (rotation != 0)
        //{
        //    float angle = rotation * rotationSpeed * Time.fixedDeltaTime;
        //    rb.MoveRotation(rb.rotation + angle);
        //}
    }
}
