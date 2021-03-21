using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0108
// This class will handle both HP bar update and health update
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : Health
{
    public float ActionSpeed
    {
        get
        {
            return actionSpeed;
        }
        set
        {
            actionSpeed = value;
            animator.speed = value;
        }
    }

    public int SpawnCost => spawnCost;
    public Vector2 ForwardVector => turnDirection == MovementState.Right ? Vector2.right : Vector2.left;
    public MovementState TurnDirection => turnDirection;
    public ObjectPool Projectile => projectilePool;
    public bool IsRanged => isRanged;
    public float AttackDamage => attackDamage;
    public AttackHitbox EnemyAttackHitbox => attackHitbox;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown / actionSpeed;
    public float AttackDelay => attackDelay / actionSpeed;
    public float CurrentCooldown { get; set; }
    public float SecondsBeforeGetUp => secondsBeforeGetUp;
    public float EnemyBaseSpeed => enemyBaseSpeed * actionSpeed;
    public float RandomSpeedFactor => randomSpeedFactor * actionSpeed;
    public float ChasingRange => chasingRange;
    public float ChasingInterval => chasingInterval;
    public bool CanSeeThroughWalls => canSeeThroughWalls;
    public Transform GroundDetector => groundDetector;
    public bool ShouldChase { get; set; }

    // How rare is this monster? The higher, the rarer
    [SerializeField]
    [Header("Enemy Spawn Cost")]
    private int spawnCost = 5;

    // Enemy AI parameters
    [Header("Enemy Movement Parameters")]
    [SerializeField]
    private float enemyBaseSpeed = 1.5f;
    [Tooltip("Real speed = Base Speed +- Random Speed Factor")]
    [SerializeField]
    private float randomSpeedFactor = 0.5f; // Actual speed = Base speed +- random factor
    [Tooltip("Range to look for and chase player")]
    [SerializeField]
    private float chasingRange = 6.0f;
    [Tooltip("How often should the enemy look for player?")]
    [SerializeField]
    private float chasingInterval = 0.5f; // Chasing every ? seconds
    [Tooltip("Can ground and walls block enemy's vision?")]
    [SerializeField]
    private bool canSeeThroughWalls = false;

    // Enemy AI ground detection system for patroling
    [Header("Ground Detection for Patroling")]
    [SerializeField]
    private Transform groundDetector;

    [Header("Health Parameters")]
    // Health system
    [SerializeField]
    private float startMaxHealth = 150.0f;
    [SerializeField]
    protected Slider hpBar;
    [SerializeField]
    private float secondsToDespawn = 2.0f;

    [Header("Super Armor Parameters")]
    // Super armor
    [SerializeField]
    private bool showSuperArmorBar = true;
    [SerializeField]
    private float maxSuperArmor = 100;
    [SerializeField]
    protected Slider superArmorBar;
    [SerializeField]
    private float secondsBeforeGetUp = 1.5f;
    [SerializeField]
    private GameObject stunStars;
    private float superArmor;

    // Animation
    private EnemyAnimation enemyAnimation;

    [Header("Attack Parameters")]
    // Attack
    [Tooltip("Is this enemy ranged or melee?")]
    [SerializeField]
    private bool isRanged = false;
    [SerializeField]
    private float attackDamage = 15.0f;
    [SerializeField]
    private float attackCooldown = 2.0f;
    [SerializeField]
    private float attackRange = 1.0f;
    [Tooltip("Delay before the attack is executed")]
    [SerializeField]
    private float attackDelay = 0.3f;

    [Header("Hitbox and projectile")]
    [Tooltip("Can leave this blank if ranged")]
    [SerializeField]
    private AttackHitbox attackHitbox;
    [Tooltip("Can leave this blank if melee")]
    [SerializeField]
    private GameObject projectilePrefab;
    private ObjectPool projectilePool;

    // Hurt effects
    [SerializeField]
    private Color hurtColor = new Color(0.75f, 0.0f, 0.0f);
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private float flashEffectDuration = 0.15f;

    // Action speed
    private float actionSpeed = 1.0f;

    // Other stuff
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private MovementState turnDirection = MovementState.Right;

    // Adapter method
    public virtual void TakeDamage(float damage, bool crit, float superArmorDamage = 0.0f, float knockUpAmplitude = 0.0f, float knockBackAmplitude = 0.0f)
    {
        if (!IsDead)
        {
            TakeDamage(damage);
            EnemyStunnedBehavior behavior = animator.GetBehaviour<EnemyStunnedBehavior>();
            if (knockUpAmplitude > 0.01f)
            {
                behavior.KnockedUp(knockUpAmplitude);
            }

            // Deal super armor damage
            if (superArmorDamage > 0.01f)
            {
                TakeSuperArmorDamage(superArmorDamage);
            }

            // Knock back
            KnockedBack(knockBackAmplitude);

            // Flinch
            Flinch();

            // Show floating damage number
            FloatingDamageManager.Instance.Spawn(damage, transform.position, crit);
            HandleHealthChange();

            // Flash hurt color
            spriteRenderer.color = hurtColor;
            CoroutineUtility.ExecDelay(() => spriteRenderer.color = originalColor, flashEffectDuration);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public void AdjustFlipping()
    {
        if (rigidbody2D.velocity.x < -0.01f)
        {
            // Is going left
            Flip(MovementState.Left);
        }
        else if (rigidbody2D.velocity.x > 0.01f)
        {
            // Is going right
            Flip(MovementState.Right);
        }
    }

    public void Flip(MovementState move)
    {
        float enemyX = transform.localScale.x;
        float hpBarX = hpBar.transform.localScale.x;
        float superArmorBarX = superArmorBar.transform.localScale.x;

        switch (move)
        {
            case MovementState.Left:
                enemyX = -Mathf.Abs(enemyX);
                // Need to flip hp bar and super armor bar as well
                hpBarX = -Mathf.Abs(hpBarX);
                superArmorBarX = -Mathf.Abs(superArmorBarX);
                turnDirection = MovementState.Left;
                break;
            case MovementState.Right:
                enemyX = Mathf.Abs(enemyX);
                // Need to flip hp bar and super armor bar as well
                hpBarX = Mathf.Abs(hpBarX);
                superArmorBarX = Mathf.Abs(superArmorBarX);
                turnDirection = MovementState.Right;
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        hpBar.transform.localScale = new Vector3(hpBarX, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
        superArmorBar.transform.localScale = new Vector3(superArmorBarX, superArmorBar.transform.localScale.y, superArmorBar.transform.localScale.z);
        transform.localScale = new Vector3(enemyX, transform.localScale.y, transform.localScale.z);
    }

    public Vector2 GetForwardVector()
    {
        switch (turnDirection)
        {
            case MovementState.Right:
                return transform.right;
            case MovementState.Left:
                return -transform.right;
            case MovementState.Idle:
                throw new System.InvalidOperationException();
            default:
                throw new System.NotImplementedException();
        }
    }

    public void ShowStunStars(bool show)
    {
        stunStars.SetActive(show);
    }

    public void RestoreSuperArmor()
    {
        superArmor = maxSuperArmor;
        HandleSuperArmorUIChange();
    }

    protected override void Start()
    {
        // GetComponents
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAnimation = new EnemyAnimation(GetComponent<Animator>());

        EnableEnemy();
    }

    protected virtual void Flinch()
    {
        enemyAnimation.PlayFlinchAnimation();
    }

    private void OnEnable()
    {
        EnableEnemy();
    }

    protected virtual void EnableEnemy()
    {
        // GetComponent
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Enemy shouldn't be stunned at first, set it to inactive
        stunStars.SetActive(false);

        // Show or not show super armor bar depends on config
        superArmorBar.gameObject.SetActive(showSuperArmorBar);

        // Set health and super armor
        base.maxHealth = startMaxHealth;
        superArmor = maxSuperArmor;

        // Set projectile if ranged
        if (isRanged && projectilePrefab != null)
        {
            projectilePool = new ObjectPool(projectilePrefab, 10);
        }

        base.Start();

        HandleHealthChange();
        HandleSuperArmorUIChange();
        hpBar.gameObject.SetActive(true);
    }

    protected override void HandleHealthChange()
    {
        // Update HP Bar
        hpBar.value = HealthPercentage;
    }

    protected override void HandleDeath()
    {
        // Reset action speed in case if changed
        actionSpeed = 1.0f;
        animator.speed = 1.0f;

        // Spawn item
        TrySpawnItem();

        // Reset color
        spriteRenderer.color = originalColor;

        // Disable health bar, super armor bar, and stun stars
        hpBar.gameObject.SetActive(false);
        superArmorBar.gameObject.SetActive(false);
        stunStars.SetActive(false);

        // Play Dead animation
        enemyAnimation.PlayDeadAnimation();

        // Destroy object in x seconds?
        StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(gameObject, secondsToDespawn));

        // Notify that this enemy is dead
        EventPublisher.TriggerEnemyDead(this);
    }

    private void TakeSuperArmorDamage(float superArmorDamage)
    {
        superArmor -= superArmorDamage;
        if (superArmor < 0.01f)
        {
            // Get knocked airborne;
            KnockedAirborne();
            superArmor = 0.0f;
        }

        // Debug.Log($"Super armor = {superArmor}");
        HandleSuperArmorUIChange();
    }

    private void KnockedAirborne()
    {
        // ebug.Log("Knocked!");
        animator.SetBool("Stunned", true);
    }

    private void KnockedBack(float amplitude)
    {
        Transform player = PlayerMovement.Instance.transform;
        rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);

        Vector2 direction;
        switch (PlayerMovement.Instance.TurnDirection)
        {
            case MovementState.Left:
                direction = Vector2.left;
                break;
            case MovementState.Right:
                direction = Vector2.right;
                break;
            default:
                throw new System.ArgumentOutOfRangeException("MovementState enum not recognized");
        }

        rigidbody2D.AddForce(amplitude * direction, ForceMode2D.Impulse);
    }

    private void HandleSuperArmorUIChange()
    {
        // Update super armor bar
        superArmorBar.value = superArmor / maxSuperArmor;
    }

    private void TrySpawnItem()
    {
        if (TryGetComponent<ItemSpawner>(out ItemSpawner spawner))
        {
            spawner.SpawnItem();
        }
    }
}
