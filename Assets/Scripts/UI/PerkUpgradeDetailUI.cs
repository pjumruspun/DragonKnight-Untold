using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkUpgradeDetailUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI headerText;

    [SerializeField]
    private TextMeshProUGUI priorText;

    [SerializeField]
    private TextMeshProUGUI afterText;

    public void SetText(string prior, string after, string header = "")
    {
        if (!string.IsNullOrEmpty(header))
        {
            // Has header
            if (header != null)
            {
                headerText.text = header;
            }
            else
            {
                Debug.LogWarning($"Warning: Tries to set header on GameObject {gameObject.name} without having headerText assigned");
            }
        }

        priorText.text = prior;
        afterText.text = after;
    }
}
