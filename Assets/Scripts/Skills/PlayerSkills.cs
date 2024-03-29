using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class PlayerSkills : ScriptableObject
{
    public IEnumerable<Sprite> GetIcons => from skill in skills select skill.skillIcon;
    public IEnumerable<float> GetBaseSkillCooldowns => from skill in skills select skill.baseCooldown;
    public IEnumerable<float> GetBaseSkillDamage => from skill in skills select skill.baseDamage;
    public bool IsCastingSkill => isCastingSkill;

    [SerializeField]
    protected Skill[] skills = new Skill[4];

    // Cache
    protected Transform transform;
    protected PlayerMovement movement;
    protected float[] currentCooldown;
    protected bool isCastingSkill = false;

    /// <summary>
    /// Returns current cooldown of every skill.
    /// currentCooldown[i] has the range of [0, stats.SkillCooldown[i]].
    /// </summary>
    /// <returns></returns>
    public virtual float[] GetCurrentCooldown()
    {
        // Need to be a method because it needs to be overriden
        return currentCooldown;
    }

    /// <summary>
    /// Returns current cooldown percentage of given skillNumber.
    /// </summary>
    /// <param name="skillNumber"></param>
    /// <returns></returns>
    public virtual float CurrentCooldownPercentage(int skillNumber)
    {
        return currentCooldown[skillNumber] / PlayerStats.Instance.SkillCooldown[skillNumber];
    }

    public virtual void Initialize(Transform transform)
    {
        // Cache
        movement = PlayerMovement.Instance;
        this.transform = transform;
        currentCooldown = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };

        // Reset casting when changing to dragon or change class
        EventPublisher.PlayerChangeClass += ResetCasting;
    }

    /// <summary>
    /// Call this in void Update() as it updates the cooldown of each skill
    /// </summary>
    public void ProcessSkillCooldown()
    {
        // Debug.Log($"{currentCooldown[0]} {currentCooldown[1]} {currentCooldown[2]} {currentCooldown[3]}");
        for (int i = 0; i < 4; ++i)
        {
            if (currentCooldown[i] > 0.0f)
            {
                currentCooldown[i] -= Time.deltaTime;
            }
            else
            {
                currentCooldown[i] = 0.0f;
            }
        }
    }

    /// <summary>
    /// Execute skill 1 (auto attack)
    /// </summary>
    public virtual void Skill1()
    {
        currentCooldown[0] = PlayerStats.Instance.SkillCooldown[0];
        isCastingSkill = true;
    }

    /// <summary>
    /// Execute skill 2
    /// </summary>
    public virtual void Skill2()
    {
        currentCooldown[1] = PlayerStats.Instance.SkillCooldown[1];
        isCastingSkill = true;
    }

    /// <summary>
    /// Execute skill 3
    /// </summary>
    public virtual void Skill3()
    {
        currentCooldown[2] = PlayerStats.Instance.SkillCooldown[2];
        isCastingSkill = true;
    }

    /// <summary>
    /// Execute ultimate skill
    /// </summary>
    public virtual void UltimateSkill()
    {
        currentCooldown[3] = PlayerStats.Instance.SkillCooldown[3];
        isCastingSkill = true;
    }

    /// <summary>
    /// Spawn a projectile which is created by "objectPool". Make sure that the projectile script has
    /// target marked as "Enemy".
    /// </summary>
    /// <param name="objectPool">an ObjectPool that has projectile prefab in it.</param>
    /// <param name="damage">How much damage does this projectile do.</param>
    /// <param name="spawnPosition">Position that the projectile should spawn at.</param>
    /// <param name="forwardVector">Player's forward vector ((-1,0) or (1,0))</param>
    /// <param name="rotationZ">Angle compared to forward vector.</param>
    /// <param name="knockUpAmplitude">If the enemy is knocked, how much force in y-axis will the enemy gets hit by when this projectile hits.</param>
    protected Projectile AttackWithProjectile(
        ref ObjectPool objectPool,
        float damage, Vector3 spawnPosition,
        Vector2 forwardVector,
        float rotationZ = 0.0f,
        float superArmorDamage = 0.0f,
        float knockUpAmplitude = 0.0f,
        float knockBackAmplitude = 0.0f,
        HitEffect hitEffect = HitEffect.None,
        SFXName sfx = SFXName.None,
        bool shouldHitContinuously = false,
        float hitInterval = 1.0f,
        bool shouldFlinch = true,
        float overrideSpeed = -1.0f
    )
    {
        GameObject spawnedObject = objectPool.SpawnObject(spawnPosition, Quaternion.identity);

        if (forwardVector.x < -0.01f)
        {
            // If left, flip then rotate
            forwardVector = Quaternion.Euler(0.0f, 0.0f, -rotationZ) * forwardVector;
        }
        else if (forwardVector.x > 0.01f)
        {
            // Don't flip
            forwardVector = Quaternion.Euler(0.0f, 0.0f, rotationZ) * forwardVector;
        }

        if (spawnedObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            PlayerStats.Instance.CalculateDamage(damage, out float finalDamage, out bool crit);
            ProjectileConfig projectileConfig = new ProjectileConfig(
                forwardVector,
                finalDamage,
                crit,
                superArmorDamage,
                knockUpAmplitude,
                knockBackAmplitude,
                hitEffect,
                sfx,
                shouldHitContinuously,
                hitInterval,
                shouldFlinch,
                overrideSpeed
            );

            projectile.SetConfig(projectileConfig);
            return projectile;
        }
        else
        {
            throw new System.InvalidOperationException();
        }
    }

    /// <summary>
    /// Melee attack, attack all enemies within the sent hitbox.
    /// </summary>
    /// <param name="desiredHitbox">Hitbox that enemies will be hit if they are in.</param>
    /// <param name="attackDamage">How much damage does this attack do.</param>
    /// <param name="superArmorDamage">If this does any super armor damage.</param>
    /// <param name="knockUpAmplitude">If the enemy is knocked, how much force in y-axis will the enemy gets hit by when this attack hits.</param>
    protected float AttackWithHitbox(
        AttackHitbox desiredHitbox,
        float attackDamage,
        float superArmorDamage = 0.0f,
        float knockUpAmplitude = 0.0f,
        float knockBackAmplitude = 0.0f,
        HitEffect hitEffect = HitEffect.None,
        SFXName sfx = SFXName.None
    )
    {
        float totalDamageDealt = 0.0f;
        bool landedCrit = false;
        HashSet<Collider2D> collidersToRemove = new HashSet<Collider2D>();
        foreach (Collider2D enemyCollider in desiredHitbox.HitColliders)
        {
            bool objectIsActive = enemyCollider.gameObject.activeInHierarchy;
            bool isReallyAnEnemy = enemyCollider.gameObject.layer == Layers.enemyLayerIndex;
            if (objectIsActive && isReallyAnEnemy)
            {
                GameObject enemyObject = enemyCollider.gameObject;
                if (enemyObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // Damage the enemy here
                    PlayerStats.Instance.CalculateDamage(attackDamage, out float finalDamage, out bool crit);
                    if (crit)
                    {
                        // Mark that this attack was crit
                        landedCrit = true;
                    }

                    enemy.TakeDamage(finalDamage, crit, superArmorDamage, knockUpAmplitude, knockBackAmplitude);
                    totalDamageDealt += finalDamage;

                    // Spawn hit effect if exist
                    HitEffectUtility.HitEffectFunction[hitEffect]?.Invoke(enemy.transform.position);
                }
                else
                {
                    // The enemy is dead, clear it from set
                    Debug.LogAssertion($"gameObject {enemyCollider.gameObject.name} does not have EnemyHealth attached to.");
                }
            }
            else
            {
                // Keep track of colliders to remove
                collidersToRemove.Add(enemyCollider);
            }
        }

        // Remove the unrelated colliders
        foreach (Collider2D unrelatedCollider in collidersToRemove)
        {
            desiredHitbox.HitColliders.Remove(unrelatedCollider);
        }

        // Play sound if dealt damage
        if (totalDamageDealt > 0.0f)
        {
            SoundManager.Play(sfx, landedCrit);
        }

        return totalDamageDealt;
    }

    protected void UnlockCastingIn(float inDuration)
    {
        CoroutineUtility.ExecDelay(() => isCastingSkill = false, inDuration);
    }

    private void ResetCasting(PlayerClass c)
    {
        isCastingSkill = false;
    }
}
