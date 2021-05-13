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
    [SerializeField]
    private TooltipTrigger tooltipTrigger;
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
        if (perkDisplayPrefab.icon != null)
        {
            perkIcon.sprite = perkDisplayPrefab.icon;
        }

        if (perkLevel == 5)
        {
            // Cannot upgrade anymore
            perkUpgradeButton.gameObject.SetActive(false);
        }

        string content = perkDisplayPrefab.description;
        string header = perkDisplayPrefab.name;
        tooltipTrigger.SetText(content, header);
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
        Debug.Log($"Soul cost = {SoulCost()}");
        // Check if player have enough soul
        if (SoulStatic.soul >= SoulCost())
        {
            // Pay soul
            SoulStatic.soul -= SoulCost();
            GameEvents.TriggerSoulChange(-SoulCost());

            // Upgrade
            PerkList.Instance.Upgrade(perkDisplayPrefab);

            if (perkLevel == 0)
            {
                PerkList.Instance.AddDevelopedPerk(perkDisplayPrefab);

                // Notify event
                GameEvents.TriggerPerkUpgrade(perkDisplayPrefab.name, 1);
                return;
            }
            else if (perkLevel == 5)
            {
                // Cannot upgrade anymore
                perkUpgradeButton.gameObject.SetActive(false);
            }

            // Notify event
            GameEvents.TriggerPerkUpgrade(perkDisplayPrefab.name, perkLevel);
        }
    }
}
