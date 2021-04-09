using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Enemy,
    Player
}

public struct ProjectileConfig
{
    public Vector2 direction;
    public float damage;
    public bool crit;
    public float superArmorDamage;
    public float knockUpAmplitude;
    public float knockBackAmplitude;
    public HitEffect hitEffect;
    public bool shouldHitContinuously;
    public float hitInterval;

    public ProjectileConfig(
        Vector2 direction,
        float damage,
        bool crit = false,
        float superArmorDamage = 0.0f,
        float knockUpAmplitude = 0.0f,
        float knockBackAmplitude = 0.0f,
        HitEffect hitEffect = HitEffect.None,
        bool shouldHitContinuously = false,
        float hitInterval = 1.0f
    )
    {
        this.direction = direction;
        this.damage = damage;
        this.crit = crit;
        this.superArmorDamage = superArmorDamage;
        this.knockUpAmplitude = knockUpAmplitude;
        this.knockBackAmplitude = knockBackAmplitude;
        this.hitEffect = hitEffect;
        this.shouldHitContinuously = shouldHitContinuously;
        this.hitInterval = hitInterval;
    }
}

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Target target;
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float maxTravelDistance = 10.0f;
    [SerializeField]
    private bool pierce = false;

    private float totalTraveledDistance = 0.0f;
    private ProjectileConfig config;

    // Continuous hit variables
    private HashSet<Collider2D> hitColliders = new HashSet<Collider2D>();
    private Coroutine hitContinuousCoroutine;

    public void SetConfig(ProjectileConfig config)
    {
        this.config = config;
        HandleRotation();

        if (config.shouldHitContinuously)
        {
            hitContinuousCoroutine = StartCoroutine(CreateHitCoroutine());
        }
    }

    private void HandleFlip()
    {
        // Hard coded projectile flip
        // Currently can only handle (-1, 0) and (1, 0) vectors
        float x = Mathf.Abs(transform.localScale.x);
        if (config.direction.x < -0.01f)
        {
            // If left, then flip left
            transform.localScale = new Vector3(-Mathf.Abs(x), transform.localScale.y, transform.localScale.z);
        }
        else if (config.direction.x > 0.01f)
        {
            // If right, then flip right
            transform.localScale = new Vector3(Mathf.Abs(x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleRotation()
    {
        // Improved version of HandleFlip()
        float degree = Vector2.Angle(Vector2.right, config.direction);
        degree = -degree;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, degree);
    }

    private void Start()
    {
        Setup();
    }

    private void OnEnable()
    {
        Setup();
    }

    private void FixedUpdate()
    {
        if (totalTraveledDistance < maxTravelDistance)
        {
            // Cast Vector2 to Vector3
            Vector3 direction3 = config.direction;
            // Travel
            transform.position += direction3 * speed * Time.fixedDeltaTime;

            // Log traveled distance
            float distanceToTravel = direction3.magnitude * speed * Time.fixedDeltaTime;
            totalTraveledDistance += distanceToTravel;
        }
        else
        {
            // Reach max distance
            // Destroy or set inactive
            Destruct();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (config.shouldHitContinuously)
        {
            // Continuous
            // Add col to HashSet instead
            hitColliders.Add(other);
        }
        else
        {
            // Non continuous
            // Deals damage here
            switch (target)
            {
                case Target.Enemy:
                    if (other.gameObject.layer == Layers.enemyLayerIndex && other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        // Deals damage to enemy
                        enemy.TakeDamage(
                            config.damage,
                            config.crit,
                            config.superArmorDamage,
                            config.knockUpAmplitude,
                            config.knockBackAmplitude
                        );

                        // Show hit effect if exist
                        HitEffectUtility.HitEffectFunction[config.hitEffect]?.Invoke(enemy.transform.position);
                    }
                    break;
                case Target.Player:
                    if (other.gameObject.layer == Layers.playerLayerIndex && other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                    {
                        playerHealth.TakeDamage(config.damage);
                    }
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            if (!pierce)
            {
                // This projectile cannot pierce
                // So destroy or set inactive after it hits
                Destruct();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (config.shouldHitContinuously)
        {
            hitColliders.Remove(other);
        }
    }

    private IEnumerator CreateHitCoroutine()
    {
        if (config.shouldHitContinuously)
        {
            while (true)
            {
                HitAllTargetsInHitbox();
                yield return new WaitForSeconds(config.hitInterval);
            }
        }
        else
        {
            throw new System.InvalidOperationException("Tried to call CreateHitCoroutine without having projectile being continuous.");
        }
    }

    private void HitAllTargetsInHitbox()
    {
        HashSet<Collider2D> collidersToRemove = new HashSet<Collider2D>();
        foreach (Collider2D collider2D in hitColliders)
        {
            bool objectIsActive = collider2D.gameObject.activeInHierarchy;
            bool shouldHit;
            if (target == Target.Player)
            {
                shouldHit = collider2D.gameObject.layer == Layers.playerLayerIndex;
            }
            else
            {
                shouldHit = collider2D.gameObject.layer == Layers.enemyLayerIndex;
            }

            if (objectIsActive && shouldHit)
            {
                GameObject enemyObject = collider2D.gameObject;
                if (enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // Damage the enemy here
                    enemy.TakeDamage(
                        config.damage,
                        config.crit,
                        config.superArmorDamage,
                        config.knockUpAmplitude,
                        config.knockBackAmplitude
                    );

                    // Spawn hit effect if exist
                    HitEffectUtility.HitEffectFunction[config.hitEffect]?.Invoke(enemy.transform.position);
                }
                else
                {
                    // The enemy is dead, clear it from set
                    Debug.LogAssertion($"gameObject {collider2D.gameObject.name} does not have EnemyHealth attached to.");
                }
            }
            else
            {
                // Keep track of colliders to remove
                collidersToRemove.Add(collider2D);
            }
        }

        // Remove the unrelated colliders
        foreach (Collider2D unrelatedCollider in collidersToRemove)
        {
            hitColliders.Remove(unrelatedCollider);
        }
    }

    private void Destruct()
    {
        config.direction = Vector2.zero;
        gameObject.SetActive(false);

        if (config.shouldHitContinuously)
        {
            // Clear colliders
            hitColliders = new HashSet<Collider2D>();
            StopCoroutine(hitContinuousCoroutine);
            hitContinuousCoroutine = null;
        }
    }

    private void Setup()
    {
        // Reset as it's activated
        totalTraveledDistance = 0.0f;

        // Set collision by layer
        switch (target)
        {
            case Target.Enemy:
                gameObject.layer = Layers.playerAttackLayerIndex;
                break;
            case Target.Player:
                gameObject.layer = Layers.enemyProjectileLayerIndex;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}
