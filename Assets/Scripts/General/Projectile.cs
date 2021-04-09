using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Enemy,
    Player
}

struct ProjectileConfig
{
    public float damage;
    public bool crit;
    public float superArmorDamage;
    public float knockUpAmplitude;
    public float knockBackAmplitude;
    public HitEffect hitEffect;

    public ProjectileConfig(
        float damage,
        bool crit,
        float superArmorDamage,
        float knockUpAmplitude,
        float knockBackAmplitude,
        HitEffect hitEffect
    )
    {
        this.damage = damage;
        this.crit = crit;
        this.superArmorDamage = superArmorDamage;
        this.knockUpAmplitude = knockUpAmplitude;
        this.knockBackAmplitude = knockBackAmplitude;
        this.hitEffect = hitEffect;
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
    private Vector2 direction;
    private float damage;
    private bool crit;
    private float superArmorDamage;
    private float knockUpAmplitude;
    private float knockBackAmplitude;
    private HitEffect hitEffect;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        HandleRotation();
    }

    public void SetDamage(float damage, bool crit)
    {
        this.damage = damage;
        this.crit = crit;
    }

    public void SetSuperArmorDamage(float superArmorDamage)
    {
        this.superArmorDamage = superArmorDamage;
    }

    public void SetKnockUpValue(float knockUpAmplitude)
    {
        this.knockUpAmplitude = knockUpAmplitude;
    }

    public void SetKnockBackValue(float knockBackAmplitude)
    {
        this.knockBackAmplitude = knockBackAmplitude;
    }

    public void SetHitEffect(HitEffect hitEffect)
    {
        this.hitEffect = hitEffect;
    }

    private void HandleFlip()
    {
        // Hard coded projectile flip
        // Currently can only handle (-1, 0) and (1, 0) vectors
        float x = Mathf.Abs(transform.localScale.x);
        if (direction.x < -0.01f)
        {
            // If left, then flip left
            transform.localScale = new Vector3(-Mathf.Abs(x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x > 0.01f)
        {
            // If right, then flip right
            transform.localScale = new Vector3(Mathf.Abs(x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleRotation()
    {
        // Improved version of HandleFlip()
        float degree = Vector2.Angle(Vector2.right, direction);
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
            Vector3 direction3 = direction;
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
        // Deals damage here
        switch (target)
        {
            case Target.Enemy:
                if (other.gameObject.layer == Layers.enemyLayerIndex && other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // Deals damage to enemy
                    enemy.TakeDamage(damage, crit, superArmorDamage, knockUpAmplitude, knockBackAmplitude);

                    // Show hit effect if exist
                    HitEffectUtility.HitEffectFunction[hitEffect]?.Invoke(enemy.transform.position);
                }
                break;
            case Target.Player:
                if (other.gameObject.layer == Layers.playerLayerIndex && other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(damage);
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

    private void Destruct()
    {
        direction = Vector2.zero;
        gameObject.SetActive(false);
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
