using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUpgradeMenu : MonoSingleton<PerkUpgradeMenu>
{
    [SerializeField]
    private GameObject perkUpgradeMenuObject;

    [SerializeField]
    private Transform perkCardsParent;

    [SerializeField]
    private GameObject perkCardPrefab;

    private const int perkCardsToGenerate = 3;
    private List<PerkUpgradeCard> perkCards = new List<PerkUpgradeCard>();

    public static void Activate()
    {
        Instance.perkUpgradeMenuObject.SetActive(true);
        Instance.SetPerksOnCards();
    }

    public static void Hide()
    {
        Instance.perkUpgradeMenuObject.SetActive(false);
    }

    private void SetPerksOnCards()
    {
        // Set perks for each card
        // First we acquire PerkTemplates from PerkRepository
        List<PerkTemplate> perkTemplates = PerkRepository.GetRandomPerks(perkCardsToGenerate);
        if (perkTemplates.Count != perkCardsToGenerate)
        {
            throw new System.Exception($"perkTemplates.Count is not equal to perkCardsToGenerate ({perkTemplates.Count}/{perkCardsToGenerate})");
        }

        // Then we assign to each card
        for (int i = 0; i < perkCardsToGenerate; ++i)
        {
            perkCards[i].SetPerk(perkTemplates[i]);
        }
    }

    private void Start()
    {
        CreateCards();
        Hide();
    }

    private void CreateCards()
    {
        // Only create on starting
        for (int i = 0; i < perkCardsToGenerate; ++i)
        {
            GameObject instantiatedCard = Instantiate(perkCardPrefab);
            instantiatedCard.transform.SetParent(perkCardsParent);
            if (instantiatedCard.TryGetComponent<PerkUpgradeCard>(out var perkUpgradeCard))
            {
                perkCards.Add(perkUpgradeCard);
            }
            else
            {
                throw new System.Exception($"Object generated from perkCardPrefab does not contain PerkUpgradeCard component.");
            }
        }

        if (perkCards.Count != perkCardsToGenerate)
        {
            throw new System.Exception($"perkCards.Count is not equal to perkCardsToGenerate ({perkCards.Count}/{perkCardsToGenerate})");
        }
    }
}
