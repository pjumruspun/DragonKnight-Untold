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
    [SerializeField]
    private GameObject upgradeDetailPrefab;
    [SerializeField]
    private Transform upgradeDetailParent;
    private int perkLevel => PerkStatic.GetPerkLevel(perkTemplate);
    private PerkTemplate perkTemplate = null;
    private List<GameObject> createdUpgradeDetails = new List<GameObject>();

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

        // Upgrade details
        foreach (var upgradeDetail in perkTemplate.perkUpgradeDetails)
        {
            GameObject instantiatedUpgradeDetailObject = Instantiate(upgradeDetailPrefab);
            createdUpgradeDetails.Add(instantiatedUpgradeDetailObject);
            instantiatedUpgradeDetailObject.transform.SetParent(upgradeDetailParent);
            if (instantiatedUpgradeDetailObject.TryGetComponent<PerkUpgradeDetailUI>(out var perkUpgradeDetailUI))
            {
                string header = upgradeDetail.Name;
                Debug.Log(header);
                PerkDisplayMethod method = upgradeDetail.DisplayMethod;
                float[] values = upgradeDetail.Values;

                string prior = "";
                string after = "";

                try
                {
                    switch (method)
                    {
                        case PerkDisplayMethod.Percentage:
                            if (perkLevel == 0)
                            {
                                prior = "0%";
                                after = (values[perkLevel] * 100).ToString() + "%";
                            }
                            else
                            {
                                prior = (values[perkLevel - 1] * 100).ToString() + "%";
                                after = (values[perkLevel] * 100).ToString() + "%";
                            }

                            break;
                        case PerkDisplayMethod.Number:
                            if (perkLevel == 0)
                            {
                                prior = "0";
                                after = (values[perkLevel]).ToString();
                            }
                            else
                            {
                                prior = (values[perkLevel - 1]).ToString();
                                after = (values[perkLevel]).ToString();
                            }

                            break;
                        default:
                            throw new System.ArgumentOutOfRangeException($"PerkDisplayMethod {method} unhandled in PerkUpgradeCard");
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    throw new System.IndexOutOfRangeException($"Values of upgrade detail {header} in perkTemplate {perkTemplate.name} must have size of {perkLevel}, instead got size of {values.Length}");
                }

                perkUpgradeDetailUI.SetText(prior, after, header);
            }
            else
            {
                throw new System.Exception($"There is not PerkUpgradeDetailUI script attached to GameObject: {instantiatedUpgradeDetailObject.name}");
            }
        }
    }

    public void UpgradeLevel()
    {
        Debug.Log($"Upgrading perk {perkTemplate.type}");
        PerkManager.Upgrade(perkTemplate);
        PerkUpgradeMenu.Hide();
    }

    private void OnDisable()
    {
        // Clear created upgrade details
        foreach (var obj in createdUpgradeDetails)
        {
            Destroy(obj);
        }

        createdUpgradeDetails = new List<GameObject>();
    }
}

