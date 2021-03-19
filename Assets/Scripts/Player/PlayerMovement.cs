using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0108
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoSingleton<PlayerMovement>
{
    public Vector2 ForwardVector => turn == MovementState.Right ? Vector2.right : Vector2.left;
    public MovementState TurnDirection => turn;

    [SerializeField]
    private bool drawJumpingRay = false;
    [SerializeField]
    private int maxDragonJumpCount = 3;
    private int dragonJumpCount = 0;
    [SerializeField]
    private float dragonGlidingSpeedMultiplier = 2.0f;
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
    private MovementState movementState = MovementState.Idle;
    private MovementState turn = MovementState.Right;

    // Lock movement stuff
    private bool isFlipLockedBySkills = false;
    private bool isMovementLockedBySkills = false;
    private bool isJumpLockedBySkills = false;
    private bool stopAllMovement = false;
    private float originalGravityScale;

    // Lock function utility
    private bool stateLock = false;
    private float lockDuration = 0.0f;

    // Forced movement
    private bool isForcedToMove = false;
    private Vector2 forceMoveDirection;
    private float forceMoveDuration = 0.0f;
    private float movingSpeed = 0.0f;
    private ForceMode2D forceMode = ForceMode2D.Force;

    // Knocked back
    private bool isKnockedBack = false;

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

    public void KnockBack(Vector2 force)
    {
        isKnockedBack = true;
        AddForceBySkill(force, true);
    }

    public void AddForceBySkill(Vector2 force, bool disableOldVy = false)
    {
        if (disableOldVy)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
        }

        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        // Experimental X movement
        // Need to find a way to properly move x
        // ForceMove(new Vector2(force.x, 0.0f), force.x * 10.0f, 1.5f, ForceMode2D.Force, groundOnly: false);
    }

    public void ForceMove(
        Vector2 direction,
        float speed,
        float duration,
        ForceMode2D forceMode = ForceMode2D.Force,
        bool groundOnly = true
    )
    {
        if (groundOnly && !isGrounded)
        {
            return;
        }

        forceMoveDirection = direction;
        isForcedToMove = true;
        forceMoveDuration = duration;
        movingSpeed = speed;
        this.forceMode = forceMode;
    }

    public void MoveForwardBySkill(
        float speed,
        float duration,
        ForceMode2D forceMode = ForceMode2D.Force,
        bool groundOnly = true
    ) => ForceMove(ForwardVector, speed, duration, forceMode, groundOnly);

    // Set reset lockers
    public void LockFlipBySkill(bool lockFlip) => isFlipLockedBySkills = lockFlip;

    public void LockMovementBySkill(bool lockMovement)
    {
        if (lockMovement)
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        this.stopAllMovement = lockMovement;
        rigidbody2D.gravityScale = lockMovement ? 0.0f : originalGravityScale;
        isMovementLockedBySkills = lockMovement;
    }

    public void LockJumpBySkill(bool lockJump) => isJumpLockedBySkills = lockJump;

    // Release automatically lockers
    public void LockFlipBySkill(float duration) => StartCoroutine(LockFlipBySkillPrivate(duration));

    public void LockMovementBySkill(float duration, bool stopAllMovement = false, bool lockFlip = true) => LockMovementBySkillPrivate(duration, stopAllMovement, lockFlip);

    public void LockJumpBySkill(float duration) => StartCoroutine(LockJumpBySkillPrivate(duration));

    private IEnumerator LockFlipBySkillPrivate(float duration)
    {
        isFlipLockedBySkills = true;
        yield return new WaitForSeconds(duration);
        isFlipLockedBySkills = false;
    }

    private void LockMovementBySkillPrivate(float duration, bool stopAllMovement, bool lockFlip)
    {
        // This function is a bit messy
        // It's coupled heavily with LockState function and ProcessLockState function
        // Need to fix some daygit s

        LockState(duration);
        if (lockFlip)
        {
            LockFlipBySkill(duration);
        }

        // Disable old movement
        rigidbody2D.velocity = Vector2.zero;
        // Stop all movement?
        this.stopAllMovement = stopAllMovement;
        // Disable gravity too
        rigidbody2D.gravityScale = 0.0f;
        // Lock movement
        isMovementLockedBySkills = true;
    }

    private IEnumerator LockJumpBySkillPrivate(float duration)
    {
        isJumpLockedBySkills = true;
        yield return new WaitForSeconds(duration);
        isJumpLockedBySkills = false;
    }

    private void UpdateIsGrounded()
    {
        // Caching IsGrounded() for each frame
        isGrounded = IsGrounded();
    }

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        originalGravityScale = rigidbody2D.gravityScale;

        // Subscribe
        EventPublisher.PlayerJump += Jump;
        EventPublisher.PlayerLand += ResetDragonJumpCount;
        EventPublisher.PlayerLand += ResetKnockbackState;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        EventPublisher.PlayerJump -= Jump;
        EventPublisher.PlayerLand -= ResetDragonJumpCount;
        EventPublisher.PlayerLand -= ResetKnockbackState;
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
            ProcessStopAllMovement();
            ProcessForceMove();
        }

        ProcessLockState();
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
        // If player skill isn't locking player's direction
        if (!isFlipLockedBySkills)
        {
            switch (movementState)
            {
                case MovementState.Right:
                    // Turn the character facing right
                    Turn(movementState);
                    break;
                case MovementState.Left:
                    // Turn left
                    Turn(movementState);
                    break;
                default:
                    break;
            }
        }
    }

    private void ProcessMovement()
    {
        if (!isMovementLockedBySkills)
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

            float velocityX = isKnockedBack ? rigidbody2D.velocity.x : 0.0f;
            if (DragonGauge.Instance.IsDragonForm && !isGrounded)
            {
                // The dragon is gliding
                velocityX = horizontalMovement * PlayerStats.Instance.MovementSpeed * dragonGlidingSpeedMultiplier;
            }
            else
            {
                velocityX = horizontalMovement * PlayerStats.Instance.MovementSpeed;
            }

            if (isKnockedBack)
            {
                if (Mathf.Abs(rigidbody2D.velocity.x) < Mathf.Abs(velocityX))
                {
                    rigidbody2D.AddForce(new Vector2(velocityX * 3.0f, 0.0f), ForceMode.Force);
                }
            }
            else
            {
                rigidbody2D.velocity = new Vector2(velocityX, rigidbody2D.velocity.y);
            }
        }
    }

    private void ProcessJump()
    {
        if (jumpKeyPressed && !isJumpLockedBySkills)
        {
            if (DragonGauge.Instance.IsDragonForm)
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
        if (jumpKeyHold && !isGrounded && rigidbody2D.velocity.y < glideFallingSpeed && DragonGauge.Instance.IsDragonForm)
        {
            // Set the falling speed to glide falling speed
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -glideFallingSpeed);
        }
    }

    private void ProcessStopAllMovement()
    {
        if (stopAllMovement)
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void ProcessLockState()
    {
        if (stateLock)
        {
            lockDuration -= Time.deltaTime;
            if (lockDuration <= 0.0f)
            {
                // Unlock
                stateLock = false;
                // Debug.Log("Unlock");

                // Unlock
                isMovementLockedBySkills = false;
                this.stopAllMovement = false;
                // Reenable gravity
                rigidbody2D.gravityScale = originalGravityScale;
                // Debug.Log("Unlock successfully");
            }
        }
    }

    private void LockState(float duration)
    {
        stateLock = true;
        lockDuration = duration;
    }

    private void ProcessForceMove()
    {
        if (isForcedToMove)
        {
            forceMoveDuration -= Time.fixedDeltaTime;
            rigidbody2D.AddForce(forceMoveDirection.normalized * movingSpeed, forceMode);
            if (forceMoveDuration <= 0.0f)
            {
                isForcedToMove = false;
                forceMoveDuration = 0.0f;
                rigidbody2D.velocity = Vector2.zero;
            }
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

    private void ResetKnockbackState()
    {
        isKnockedBack = false;
    }

    private void Turn(MovementState side)
    {
        float x = 0.0f;
        switch (side)
        {
            case MovementState.Right:
                x = Mathf.Abs(transform.localScale.x);
                break;
            case MovementState.Left:
                x = -Mathf.Abs(transform.localScale.x);
                break;
            case MovementState.Idle:
                throw new System.InvalidOperationException();
            default:
                throw new System.NotImplementedException();
        }

        turn = side;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
