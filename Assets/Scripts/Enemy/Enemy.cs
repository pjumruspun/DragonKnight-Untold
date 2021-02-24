using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will handle both HP bar update and health update
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

    // Animation
    private EnemyAnimation enemyAnimation;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        // Show floating damage number
        if (!IsDead)
        {
            FloatingDamageManager.Instance.Spawn(damage, transform.position);
            HandleHealthChange();
        }
    }

    protected override void Start()
    {
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
        Destroy(gameObject, 2.0f);
    }
}
