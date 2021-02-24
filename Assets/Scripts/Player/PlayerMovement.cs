using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    [SerializeField]
    private bool drawJumpingRay = false;
    [SerializeField]
    private float baseMovementSpeed = 3.0f;
    [SerializeField]
    private int maxDragonJumpCount = 3;
    private int dragonJumpCount = 0;
    [SerializeField]
    private float dragonGlidingSpeed = 8.0f;
    [SerializeField]
    private float jumpingForce = 7.0f;
    [SerializeField]
    private float distanceToGround = 0.6f;
    [SerializeField]
    private float glideFallingSpeed = 1.0f;
    private const float expandedColliderFactor = 1.0f; // 1.0f = full collider size
    private Rigidbody2D rigidbody2D;
    private bool jumpKeyPressed = false;
    private bool jumpKeyHold = false;
    private bool isGrounded = false;
    private bool lastFrameWasGrounded = false;
    private enum MovementState
    {
        Right,
        Left,
        Idle
    }
    private MovementState movementState = MovementState.Idle;

    // This could be cached and put private
    // Letting other classes access through property instead
    public bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = distanceToGround;

        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D col))
        {
            // Shoot 3 rays according to the size of the box collider
            float colliderSizeX = col.size.x;

            // Expander is expandedColliderFactor % of the collider size
            Vector2 expander = Vector2.right * colliderSizeX * expandedColliderFactor / 2;
            Vector2 positionRight = position + expander;
            Vector2 positionLeft = position - expander;

            if (drawJumpingRay)
            {
                // Draw ray left, center, and right
                Debug.DrawRay(positionRight, direction * distance, Color.green);
                Debug.DrawRay(position, direction * distance, Color.green);
                Debug.DrawRay(positionLeft, direction * distance, Color.blue);
            }

            // Raycast to the ground layer only
            RaycastHit2D hitLeft = Physics2D.Raycast(positionLeft, direction, distance, Layers.GroundLayer);
            RaycastHit2D hitCenter = Physics2D.Raycast(position, direction, distance, Layers.GroundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(positionRight, direction, distance, Layers.GroundLayer);

            // Grounded if left OR right OR center touches with the ground
            return hitLeft.collider != null || hitRight.collider != null || hitCenter.collider != null;
        }
        else
        {
            // Cannot find collider, shoot ray only from the center
            if (drawJumpingRay)
            {
                Debug.DrawRay(position, direction * distance, Color.red);
            }

            // Raycast to the ground layer only
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, Layers.GroundLayer);
            return hit.collider != null;
        }
    }

    private void UpdateIsGrounded()
    {
        // Caching IsGrounded() for each frame
        isGrounded = IsGrounded();
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Subscribe
        EventPublisher.PlayerJump += Jump;
        EventPublisher.PlayerLand += ResetDragonJumpCount;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerJump -= Jump;
        EventPublisher.PlayerLand -= ResetDragonJumpCount;
    }

    // Update for listening to input
    private void Update()
    {
        ListenInput();
        UpdateIsGrounded();
    }

    // FixedUpdate for any physics related events
    private void FixedUpdate()
    {
        if (!PlayerHealth.Instance.IsDead)
        {
            ProcessFlipping();
            ProcessMovement();
            ProcessJump();
            ProcessPlayerLanding();
            ProcessGlide();
        }
    }

    private void ListenInput()
    {
        // Jump
        if (InputManager.Up)
        {
            jumpKeyPressed = true;
        }

        // Glide
        if (InputManager.HoldUp)
        {
            jumpKeyHold = true;
        }
        else
        {
            jumpKeyHold = false;
        }

        // Right
        if (InputManager.Right)
        {
            movementState = MovementState.Right;
        }

        // Left
        if (InputManager.Left)
        {
            movementState = MovementState.Left;
        }

        // Stop moving if not pressing
        if (!InputManager.Right && !InputManager.Left)
        {
            movementState = MovementState.Idle;
        }
    }

    private void ProcessFlipping()
    {
        switch (movementState)
        {
            case MovementState.Right:
                // Turn the character facing right
                TurnRight(true);
                break;
            case MovementState.Left:
                // Turn left
                TurnRight(false);
                break;
            default:
                break;
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

        float velocityX = 0.0f;
        if (PlayerAbilities.Instance.IsDragonForm && !isGrounded)
        {
            // The dragon is gliding
            velocityX = horizontalMovement * dragonGlidingSpeed;
        }
        else
        {
            velocityX = horizontalMovement * baseMovementSpeed;
        }

        rigidbody2D.velocity = new Vector2(velocityX, rigidbody2D.velocity.y);
    }

    private void ProcessJump()
    {
        // if (!isGrounded)
        // {
        //     jumpKeyPressed = false;
        // }

        if (jumpKeyPressed)
        {
            if (PlayerAbilities.Instance.IsDragonForm)
            {
                // Player is in dragon form and wants to jump
                if (dragonJumpCount < maxDragonJumpCount)
                {
                    EventPublisher.TriggerPlayerJump();
                    ++dragonJumpCount;
                }
                else
                {
                    // Player cannot jump, reset the jumpKeyPressed variable
                    // So that player doesn't queue up jumping action
                    jumpKeyPressed = false;
                }
            }
            else
            {
                if (isGrounded)
                {
                    // Player is in human form and wants to jump
                    EventPublisher.TriggerPlayerJump();
                }
                else
                {
                    // Player cannot jump, reset the jumpKeyPressed variable
                    // So that player doesn't queue up jumping action
                    jumpKeyPressed = false;
                }
            }
        }
    }

    private void ProcessGlide()
    {
        // If jump key is hold and player is in the air
        // With falling speed more than glide falling speed
        // And if player is in dragon mode
        if (jumpKeyHold && !isGrounded && rigidbody2D.velocity.y < glideFallingSpeed && PlayerAbilities.Instance.IsDragonForm)
        {
            // Set the falling speed to glide falling speed
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -glideFallingSpeed);
        }
    }

    private void Jump()
    {
        // Jump
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpingForce);
        jumpKeyPressed = false;
    }

    private void ResetDragonJumpCount()
    {
        // Triggered when player lands
        dragonJumpCount = 0;
    }

    private void ProcessPlayerLanding()
    {
        // If last frame wasn't on ground,
        // but this frame is
        if (!lastFrameWasGrounded && isGrounded)
        {
            // Means that player just landed on the ground
            EventPublisher.TriggerPlayerLand();
        }

        lastFrameWasGrounded = isGrounded;
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
