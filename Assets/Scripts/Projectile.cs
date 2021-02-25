using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Enemy,
    Player
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

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
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
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        // Deals damage here
        switch (target)
        {
            case Target.Enemy:
                if (other.gameObject.layer == Layers.enemyLayerIndex && other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // Deals damage to enemy
                    enemy.TakeDamage(damage);
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
            gameObject.SetActive(false);
        }
    }

    private void Setup()
    {
        // Reset as it's activated
        totalTraveledDistance = 0.0f;

        // Hard coded projectile flip
        // Currently can only handle (-1, 0) and (1, 0) vectors
        float x = Mathf.Abs(transform.localScale.x);
        if (direction.x < -0.01f)
        {
            // If left, then flip
            transform.localScale = new Vector3(-Mathf.Abs(x), transform.localScale.y, transform.localScale.z);
        }

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
