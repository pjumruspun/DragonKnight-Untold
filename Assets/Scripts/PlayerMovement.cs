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
    private bool lastFrameWasGrounded = false;
    private enum MovementState
    {
        Right,
        Left,
        Idle
    }
    private MovementState movementState = MovementState.Idle;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // Subscribe
        EventPublisher.PlayerJump += Jump;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerJump -= Jump;
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
        ProcessPlayerLanding();
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
            movementState = MovementState.Right;
            // Turn the character facing right
            TurnRight(true);
        }

        // Left
        if (Input.GetKey(KeyCode.A))
        {
            movementState = MovementState.Left;
            // Turn left
            TurnRight(false);
        }

        // Stop moving if not pressing
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            movementState = MovementState.Idle;
        }
    }

    private void ProcessMovement()
    {
        // Move according to input
        float horizontalMovement = 0.0f;
        switch (movementState)
        {
            case MovementState.Right:
                horizontalMovement = 1.0f;
                EventPublisher.TriggerPlayerRun();
                break;
            case MovementState.Left:
                horizontalMovement = -1.0f;
                EventPublisher.TriggerPlayerRun();
                break;
            default:
                EventPublisher.TriggerPlayerStop();
                break;
        }

        float velocityX = horizontalMovement * baseMovementSpeed;
        rb2D.velocity = new Vector2(velocityX, rb2D.velocity.y);
    }

    private void ProcessJump()
    {
        if (jumpKeyPressed)
        {
            // Trigger player jump
            EventPublisher.TriggerPlayerJump();
        }
    }

    private void Jump()
    {
        // Jump
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpingForce);
        jumpKeyPressed = false;
    }

    private void ProcessPlayerLanding()
    {
        // If last frame wasn't on ground,
        // but this frame is
        if (!lastFrameWasGrounded && IsGrounded())
        {
            // Means that player just landed on the ground
            Debug.Log("Landed");
            EventPublisher.TriggerPlayerLand();
        }

        lastFrameWasGrounded = IsGrounded();
    }

    private bool IsGrounded()
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
