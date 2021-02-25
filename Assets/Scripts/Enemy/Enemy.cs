using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will handle both HP bar update and health update
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : Health
{
    public float EnemyBaseSpeed => enemyBaseSpeed;
    public float RandomSpeedFactor => randomSpeedFactor;
    public float ChasingRange => chasingRange;
    public float ChasingInterval => chasingInterval;
    public bool CanSeeThroughWalls => canSeeThroughWalls;
    public Transform GroundDetector => groundDetector;
    [HideInInspector]
    public bool ShouldChase { get; set; }

    // Enemy AI parameters
    [SerializeField]
    private float enemyBaseSpeed = 1.5f;
    [SerializeField]
    private float randomSpeedFactor = 0.5f; // Actual speed = Base speed +- random factor
    [SerializeField]
    private float chasingRange = 6.0f;
    [SerializeField]
    private float chasingInterval = 0.5f; // Chasing every ? seconds
    [SerializeField]
    private bool canSeeThroughWalls = false;

    // Enemy AI ground detection system for patroling
    [SerializeField]
    private Transform groundDetector;

    // Health system
    [SerializeField]
    private float enemyMaxHealth = 150.0f;
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private float secondsToDespawn = 2.0f;

    // Animation
    private EnemyAnimation enemyAnimation;

    // Other stuff
    private Rigidbody2D rigidbody2D;

    // Adapter method
    public void TakeDamage(float damage, bool crit)
    {
        if (!IsDead)
        {
            TakeDamage(damage);
            // Show floating damage number
            FloatingDamageManager.Instance.Spawn(damage, transform.position, crit);
            HandleHealthChange();
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public void AdjustRotation()
    {
        float enemyX = transform.localScale.x;
        float hpBarX = hpBar.transform.localScale.x;

        if (rigidbody2D.velocity.x < -0.01f)
        {
            // Is going left
            enemyX = -Mathf.Abs(enemyX);
            // Need to flip hp bar as well
            hpBarX = -Mathf.Abs(hpBarX);
        }
        else if (rigidbody2D.velocity.x > 0.01f)
        {
            // Is going right
            enemyX = Mathf.Abs(enemyX);
            // Need to flip hp bar as well
            hpBarX = Mathf.Abs(hpBarX);
        }

        hpBar.transform.localScale = new Vector3(hpBarX, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
        transform.localScale = new Vector3(enemyX, transform.localScale.y, transform.localScale.z);
    }

    protected override void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
        maxHealth = enemyMaxHealth;
        base.Start();
    }

    protected override void HandleHealthChange()
    {
        // Update HP Bar
        hpBar.value = currentHealth / maxHealth;
    }

    protected override void HandleDeath()
    {
        // Disable health bar
        hpBar.gameObject.SetActive(false);

        // Play Dead animation
        enemyAnimation.PlayDeadAnimation();

        // Destroy object in x seconds?
        StartCoroutine(CoroutineUtility.Instance.HideAfterSeconds(gameObject, secondsToDespawn));
    }
}
