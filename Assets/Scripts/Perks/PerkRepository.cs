using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all types of perks
public class PerkRepository : MonoSingleton<PerkRepository>
{
    [SerializeField]
    private List<Perk> giftedPerk;
    [SerializeField]
    private List<Perk> developedPerk;
    [SerializeField]
    private List<Perk> wearablePerk;

    public static List<Perk> GetRandomGiftedPerk()
    {
        float[] probNumOfPerk = {0.15F, 0.95F, 1.0F};
        System.Random rnd = new System.Random();

        int sumOfChance = 0;
        for(int i = 0; i < Instance.giftedPerk.Count; i++)
        {
            sumOfChance += Instance.giftedPerk[i].Chance;
        }
        
        List<float> probGiftedPerk = new List<float>();
        float cumuProb = 0;
        for(int i = 0; i < Instance.giftedPerk.Count; i++)
        {
            cumuProb += (float)Instance.giftedPerk[i].Chance / (float)sumOfChance;
            probGiftedPerk.Add(cumuProb);
        }

        float prob = (float)rnd.NextDouble();
        int numberOfPerk = 0;
        for(int i = 0; i < probNumOfPerk.Length; i++)
        {
            if(prob < probNumOfPerk[i]) 
            {
                numberOfPerk = i + 1;
                break;
            }
            
        }

        int loopTime = 0;
        int maxLoopTime = 10000;
        List<Perk> playerPerk = new List<Perk>();
        while(numberOfPerk > 0)
        {
            double perkProb = rnd.NextDouble();
            for(int i = 0; i < probGiftedPerk.Count; i++)
            {
                if(perkProb < probGiftedPerk[i] && !playerPerk.Contains(Instance.giftedPerk[i])) 
                {
                    playerPerk.Add(Instance.giftedPerk[i]);
                    numberOfPerk -= 1;
                    break;
                }
            }
            loopTime += 1;
            if(loopTime == maxLoopTime)
            {
                break;
            }
        }

        return playerPerk;
    }
}
