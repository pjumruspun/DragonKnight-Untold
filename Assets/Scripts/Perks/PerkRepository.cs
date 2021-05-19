using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Contains all types of perks
public class PerkRepository : MonoSingleton<PerkRepository>
{
    [SerializeField]
    private List<PerkTemplate> sTemplates;

    [SerializeField]
    private List<PerkTemplate> aTemplates;

    [SerializeField]
    private List<PerkTemplate> bTemplates;

    private const float sTierChance = 0.20f;
    private const float aTierChance = 0.35f;
    private const float bTierChance = 0.45f;
    private const int maxAttempts = 400;

    public static List<PerkTemplate> GetRandomPerks(int amount)
    {
        List<PerkTemplate> templates = new List<PerkTemplate>();
        int attempt = 0;

        while (templates.Count < amount && attempt < maxAttempts)
        {
            float random = Random.Range(0.0f, 1.0f);
            PerkTemplate templateToAdd;
            if (random < sTierChance)
            {
                templateToAdd = GetRandomPerkFromList(Instance.sTemplates);
            }
            else if (random < sTierChance + aTierChance)
            {
                templateToAdd = GetRandomPerkFromList(Instance.aTemplates);
            }
            else if (random < sTierChance + aTierChance + bTierChance)
            {
                templateToAdd = GetRandomPerkFromList(Instance.bTemplates);
            }
            else
            {
                float maxValue = sTierChance + aTierChance + bTierChance;
                throw new System.Exception($"Error in PerkRepository.GetRandomPerk(): random value exceed maximum value ({random} > {maxValue})");
            }

            ++attempt;
            if (ContainsPerk(templates, templateToAdd.type))
            {
                // We will try again
                continue;
            }
            else
            {
                // Add
                templates.Add(templateToAdd);
            }
        }

        return templates;
    }

    private static PerkTemplate GetRandomPerkFromList(List<PerkTemplate> templates)
    {
        if (templates.Count < 1)
        {
            throw new System.Exception($"Error in PerkRepository: templates has length < 0");
        }

        // Make sure the perk returned can be upgraded
        List<PerkTemplate> results = new List<PerkTemplate>();
        foreach (var template in templates)
        {
            int perkLevel = PerkStatic.GetPerkLevel(template);
            if (perkLevel < template.maxPerkLevel)
            {
                results.Add(template);
            }
            else
            {
                Debug.Log($"{template.type} is at max level {perkLevel}/{template.maxPerkLevel}");
            }
        }

        if (results.Count < 1)
        {
            throw new System.Exception($"Error in PerkRepository: results has length < 0");
        }

        System.Random random = new System.Random();
        int index = random.Next(results.Count);
        return results[index];
    }

    private static bool ContainsPerk(IList<PerkTemplate> templates, PerkType perkType)
    {
        foreach (var template in templates)
        {
            if (template.type == perkType)
            {
                return true;
            }
        }

        return false;
    }

    // public static List<PerkTemplate> GetRandomGiftedPerk()
    // {
    //     float[] probNumOfPerk = { 0.15F, 0.95F, 1.0F };
    //     System.Random rnd = new System.Random();

    //     int sumOfChance = 0;
    //     for (int i = 0; i < Instance.giftedPerk.Count; i++)
    //     {
    //         sumOfChance += Instance.giftedPerk[i].Chance;
    //     }

    //     List<float> probGiftedPerk = new List<float>();
    //     float cumuProb = 0;
    //     for (int i = 0; i < Instance.giftedPerk.Count; i++)
    //     {
    //         cumuProb += (float)Instance.giftedPerk[i].Chance / (float)sumOfChance;
    //         probGiftedPerk.Add(cumuProb);
    //     }

    //     float prob = (float)rnd.NextDouble();
    //     int numberOfPerk = 0;
    //     for (int i = 0; i < probNumOfPerk.Length; i++)
    //     {
    //         if (prob < probNumOfPerk[i])
    //         {
    //             numberOfPerk = i + 1;
    //             break;
    //         }

    //     }

    //     int loopTime = 0;
    //     int maxLoopTime = 10000;
    //     List<PerkTemplate> playerPerk = new List<PerkTemplate>();
    //     while (numberOfPerk > 0)
    //     {
    //         double perkProb = rnd.NextDouble();
    //         for (int i = 0; i < probGiftedPerk.Count; i++)
    //         {
    //             if (perkProb < probGiftedPerk[i] && !playerPerk.Contains(Instance.giftedPerk[i]))
    //             {
    //                 PerkTemplate newPerk = new PerkTemplate(Instance.giftedPerk[i]);
    //                 newPerk.PerkLevel = 1;
    //                 playerPerk.Add(newPerk);
    //                 numberOfPerk -= 1;
    //                 break;
    //             }
    //         }
    //         loopTime += 1;
    //         if (loopTime == maxLoopTime)
    //         {
    //             break;
    //         }
    //     }

    //     return playerPerk;
    // }
}
