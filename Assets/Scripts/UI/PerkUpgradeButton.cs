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

    void Start()
    {
        if (perkDisplayPrefab.icon != null)
        {
            perkIcon.sprite = perkDisplayPrefab.icon;
        }

    }

    void Update()
    {
        perkLevelText.text = PerkListStatic.GetPerkLevel(perkDisplayPrefab).ToString();
    }

    public void UpgradeLevel()
    {
        Debug.Log("upgrade perk" + perkDisplayPrefab.name);
        PerkList.Instance.Upgrade(perkDisplayPrefab);
        if (PerkListStatic.GetPerkLevel(perkDisplayPrefab) == 0)
        {
            PerkList.Instance.AddDevelopedPerk(perkDisplayPrefab);
        }
    }
}
