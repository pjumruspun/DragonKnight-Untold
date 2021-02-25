using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will handle both HP bar update and health update
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : Health
{
    public float SecondsBeforeGetUp => secondsBeforeGetUp;
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

    // Super armor
    [SerializeField]
    private bool showSuperArmorBar = true;
    [SerializeField]
    private float maxSuperArmor = 100;
    [SerializeField]
    private Slider superArmorBar;
    [SerializeField]
    private float secondsBeforeGetUp = 1.5f;
    [SerializeField]
    private GameObject stunStars;
    private float superArmor;

    // Animation
    private EnemyAnimation enemyAnimation;

    // Other stuff
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    // Adapter method
    public void TakeDamage(float damage, bool crit, float superArmorDamage = 0.0f)
    {
        if (!IsDead)
        {
            TakeDamage(damage);
            // Deal super armor damage
            if (superArmorDamage > 0.01f)
            {
                TakeSuperArmorDamage(superArmorDamage);
            }

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
        stunStars.SetActive(false);
        animator = GetComponent<Animator>();
        superArmorBar.gameObject.SetActive(showSuperArmorBar);
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAnimation = new EnemyAnimation(GetComponent<Animator>());
        maxHealth = enemyMaxHealth;
        superArmor = maxSuperArmor;
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

    private void TakeSuperArmorDamage(float superArmorDamage)
    {
        superArmor -= superArmorDamage;
        if (superArmor < 0.01f)
        {
            // Get knocked airborne;
            KnockedAirborne();
            superArmor = 0.0f;
        }

        Debug.Log($"Super armor = {superArmor}");
        HandleSuperArmorUIChange();
    }

    private void KnockedAirborne()
    {
        Debug.Log("Knocked!");
        animator.SetBool("Stunned", true);
    }

    private void HandleSuperArmorUIChange()
    {
        // Update super armor bar
        superArmorBar.value = superArmor / maxSuperArmor;
    }
}
