using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkUpgradeButton : MonoBehaviour
{
    [SerializeField]
    private Perk perkDisplayPrefab;
    [SerializeField]
    private Image perkIcon;
    [SerializeField]
    private Text perkLevelText;
    [SerializeField]
    private Text soulCostText;
    [SerializeField]
    private Button perkUpgradeButton;
    private int perkLevel => PerkListStatic.GetPerkLevel(perkDisplayPrefab);
    private int SoulCost()
    {
        if (perkLevel < 5)
        {
            return perkDisplayPrefab.soulToUpgrade[perkLevel];
        }

        return -1;
    }

    private void Start()
    {
        Debug.Log(perkLevel);
        Debug.Log(perkDisplayPrefab.soulToUpgrade[0]);
        if (perkDisplayPrefab.icon != null)
        {
            perkIcon.sprite = perkDisplayPrefab.icon;
        }

        if (perkLevel == 5)
        {
            // Cannot upgrade anymore
            perkUpgradeButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Level text
        string level = perkLevel.ToString();
        perkLevelText.text = $"Lv {level}/5";

        // Soul cost text
        if (perkLevel < 5)
        {
            soulCostText.text = $"Cost: {SoulCost()} Soul";
        }
        else
        {
            soulCostText.text = $"MAX LEVEL";
        }
    }

    public void UpgradeLevel()
    {
        // Check if player have enough soul
        if (SoulStatic.soul > SoulCost())
        {
            // Upgrade
            PerkList.Instance.Upgrade(perkDisplayPrefab);
            Debug.Log(perkLevel);
            // Pay soul
            SoulStatic.soul -= SoulCost();
            GameEvents.TriggerSoulChange();

            if (perkLevel == 0)
            {
                PerkList.Instance.AddDevelopedPerk(perkDisplayPrefab);
            }
            else if (perkLevel == 5)
            {
                // Cannot upgrade anymore
                perkUpgradeButton.gameObject.SetActive(false);
            }
        }
    }
}
