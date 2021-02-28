using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonGaugeUI : MonoBehaviour
{
    [SerializeField]
    private bool showText = true;
    [SerializeField]
    private Image dragonFill;
    [SerializeField]
    private Text dragonText;

    private void Start()
    {
        dragonText.gameObject.SetActive(showText);
        EventPublisher.DragonGaugeChange += UpdateDragonBar;
        EventPublisher.DragonGaugeChange += UpdateDragonGaugeText;
    }

    private void OnDestroy()
    {
        EventPublisher.DragonGaugeChange -= UpdateDragonBar;
        EventPublisher.DragonGaugeChange -= UpdateDragonGaugeText;
    }

    private void UpdateDragonBar(float current)
    {
        float maxDragonEnergy = DragonGauge.Instance.MaxDragonEnergy;
        dragonFill.fillAmount = current / maxDragonEnergy;
    }

    private void UpdateDragonGaugeText(float current)
    {
        float maxDragonEnergy = DragonGauge.Instance.MaxDragonEnergy;
        dragonText.text = $"{Mathf.Round(current)}/{maxDragonEnergy}";
    }
}
