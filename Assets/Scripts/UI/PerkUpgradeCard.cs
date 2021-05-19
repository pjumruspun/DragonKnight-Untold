using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkUpgradeCard : MonoBehaviour
{
    [SerializeField]
    private Image perkIcon;
    [SerializeField]
    private TextMeshProUGUI perkName;
    [SerializeField]
    private TextMeshProUGUI perkDescription;
    [SerializeField]
    private TextMeshProUGUI perkLevelTextBefore;
    [SerializeField]
    private TextMeshProUGUI perkLevelTextAfter;
    [SerializeField]
    private Button perkUpgradeButton;
    private int perkLevel => PerkStatic.GetPerkLevel(perkTemplate);
    private PerkTemplate perkTemplate = null;

    public void SetPerk(PerkTemplate perkTemplate)
    {
        this.perkTemplate = perkTemplate;

        // Should also update information
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (perkTemplate == null)
        {
            throw new System.NullReferenceException("perkTemplate is null");
        }

        // Set icon        
        if (perkTemplate.icon != null)
        {
            perkIcon.sprite = perkTemplate.icon;
        }

        // Name and description
        string description = perkTemplate.description;
        string name = perkTemplate.name;

        perkDescription.text = description;
        perkName.text = name;

        // Level text
        string level = perkLevel.ToString();
        perkLevelTextBefore.text = $"Lv{level}";
        perkLevelTextAfter.text = $"Lv{(perkLevel + 1).ToString()}";
    }

    public void UpgradeLevel()
    {
        Debug.Log($"Upgrading perk {perkTemplate.type}");
        PerkManager.Upgrade(perkTemplate);
        PerkUpgradeMenu.Hide();
    }
}

