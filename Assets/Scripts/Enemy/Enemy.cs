using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class will handle both HP bar update and health update
[RequireComponent(typeof(Animator))]
public class Enemy : Health
{
    [SerializeField]
    private float enemyMaxHealth = 150.0f;
    [SerializeField]
    private Slider hpBar;
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
        // Play Dead animation
        enemyAnimation.PlayDeadAnimation();

        // Destroy object in x seconds?
        Destroy(gameObject, 2.0f);
    }
}
