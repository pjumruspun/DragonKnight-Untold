using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkUpgradeButton : MonoBehaviour
{
    [SerializeField]
    private Button perkUpgradeButton;

    [SerializeField]
    private TooltipTrigger tooltip;

    [SerializeField]
    private TextMeshProUGUI useText;

    private string notEnoughPotionText = "You do not have any perk potion!".Color("F01010");
    private string enoughPotionText = "Use one perk potion to gain a perk!".Color("10F010");
    private Color originalUseTextColor;
    private Color disabledUseTextColor = new Color(0.5f, 0.5f, 0.5f);

    public void ShowUpgradeMenu()
    {
        PerkUpgradeMenu.Activate();
    }

    private void Start()
    {
        perkUpgradeButton.onClick.AddListener(ShowUpgradeMenu);
        originalUseTextColor = useText.color;
    }

    private void Update()
    {
        ProcessTokenAmount();
    }

    private void ProcessTokenAmount()
    {
        if (PerkStatic.upgradeToken < 1)
        {
            perkUpgradeButton.interactable = false;
            tooltip.SetText(content: notEnoughPotionText);
            useText.color = disabledUseTextColor;
        }
        else
        {
            perkUpgradeButton.interactable = true;
            tooltip.SetText(content: enoughPotionText);
            useText.color = originalUseTextColor;
        }
    }
}
