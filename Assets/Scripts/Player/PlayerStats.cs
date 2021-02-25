using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    // Visible stats
    private int atk = 0;
    private int agi = 0;
    private int vit = 0;
    private int tal = 0;
    private int luk = 0;

    // Hidden stats
    private float critDamage = 1.5f;
    private float healthRegen = 1.0f;

    public PlayerStats()
    {

    }

    public PlayerStats(int atk, int agi, int vit, int tal, int luk)
    {
        this.atk = atk;
        this.agi = agi;
        this.vit = vit;
        this.tal = tal;
        this.luk = luk;
    }

    // Assign stats but not create new object
    public void AssignStats(int atk, int agi, int vit, int tal, int luk)
    {
        this.atk = atk;
        this.agi = agi;
        this.vit = vit;
        this.tal = tal;
        this.luk = luk;
    }

    public void CalculateDamage(float baseDamage, out float finalDamage, out bool crit, bool canCrit = true)
    {
        float random = Random.Range(0.0f, 1.0f);

        // Damage +3% for each atk
        float damage = baseDamage * (1 + 0.03f * atk);

        // Handle crit
        if (random < CalculateCritChance() && canCrit)
        {
            // Crit
            finalDamage = damage * critDamage;
            crit = true;
        }
        else
        {
            // Didn't crit
            finalDamage = damage;
            crit = false;
        }
    }

    public float CalculateCritChance()
    {
        return luk * 0.02f;
    }

    public override string ToString()
    {
        return $"ATK: {atk}\t AGI: {agi}\t VIT: {vit}\t TAL: {tal}\t LUK: {luk}";
    }
}
