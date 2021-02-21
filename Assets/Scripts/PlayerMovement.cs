using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool drawJumpingRay = false;
    [SerializeField]
    private float baseMovementSpeed = 3.0f;
    [SerializeField]
    private float jumpingForce = 5.0f;
    [SerializeField]
    private float distanceToGround = 0.6f;
    private Rigidbody2D rb2D;
    private LayerMask groundLayer = 1 << groundLayerIndex;
    private const int groundLayerIndex = 7; // Layer "Ground"
    private bool jumpKeyPressed = false;
    private float horizontalMovement = 0.0f;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update for monitoring input
    private void Update()
    {
        ProcessInput();
    }

    // FixedUpdate for any physics related events
    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessJump();
    }

    private void ProcessInput()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            jumpKeyPressed = true;
        }

        // Right
        if (Input.GetKey(KeyCode.D))
        {
            horizontalMovement = 1.0f;
            // Turn the character facing right
            TurnRight(true);
        }

        // Left
        if (Input.GetKey(KeyCode.A))
        {
            horizontalMovement = -1.0f;
            // Turn left
            TurnRight(false);
        }

        // Stop moving if not pressing
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            horizontalMovement = 0.0f;
        }
    }

    private void ProcessMovement()
    {
        // Move according to input
        float velocityX = horizontalMovement * baseMovementSpeed;
        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);
    }

    private void ProcessJump()
    {
        if (jumpKeyPressed)
        {
            // Jump
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpingForce);
            jumpKeyPressed = false;
        }
    }

    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = distanceToGround;
        if (drawJumpingRay)
        {
            Debug.DrawRay(position, direction * distance, Color.green);
        }

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        return hit.collider != null;
    }

    private void TurnRight(bool right)
    {
        float x = 0.0f;
        if (right)
        {
            x = Mathf.Abs(transform.localScale.x);
        }
        else
        {
            x = -Mathf.Abs(transform.localScale.x);
        }

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
