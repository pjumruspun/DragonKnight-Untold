using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private bool showHPText = true;

    [SerializeField]
    private Image healthFill;

    [SerializeField]
    private Text hpText;

    private void Start()
    {
        hpText.gameObject.SetActive(showHPText);
        EventPublisher.PlayerHealthChange += UpdateHPBar;
        EventPublisher.PlayerHealthChange += UpdateHPText;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerHealthChange -= UpdateHPBar;
        EventPublisher.PlayerHealthChange -= UpdateHPText;
    }

    private void UpdateHPBar()
    {
        float currentHealth = PlayerHealth.Instance.CurrentHealth;
        float maxHealth = PlayerHealth.Instance.MaxHealth;
        healthFill.fillAmount = currentHealth / maxHealth;
    }

    private void UpdateHPText()
    {
        float currentHealth = PlayerHealth.Instance.CurrentHealth;
        float maxHealth = PlayerHealth.Instance.MaxHealth;
        hpText.text = $"{Mathf.Ceil(currentHealth)}/{maxHealth}";
    }
}
