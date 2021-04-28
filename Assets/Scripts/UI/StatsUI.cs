using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject statsUIObject;

    [SerializeField]
    private Text atk;

    [SerializeField]
    private Text agi;

    [SerializeField]
    private Text vit;

    [SerializeField]
    private Text tal;

    [SerializeField]
    private Text luk;

    [SerializeField]
    private Text critDamage;

    [SerializeField]
    private Text movementSpeed;

    [SerializeField]
    private Text healthRegen;

    [SerializeField]
    private Text attackSpeed;

    [SerializeField]
    private Text cooldownReduction;

    private void Start()
    {
        CoroutineUtility.ExecDelay(() =>
        {
            UpdateUI();
            statsUIObject.SetActive(false);
        }, Time.deltaTime);

        EventPublisher.PlayerStatsChange += UpdateUI;
    }

    private void Update()
    {
        ListenToStatsWindowSignal();
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerStatsChange -= UpdateUI;
    }

    private void ListenToStatsWindowSignal()
    {
        if (InputManager.StatsWindow)
        {
            statsUIObject.SetActive(!statsUIObject.activeInHierarchy);
        }
    }

    private void UpdateUI()
    {
        Debug.Log(atk.text);
        Debug.Log(PlayerStats.Instance.ATK);
        atk.text = $"ATK: {PlayerStats.Instance.ATK}";
        agi.text = $"AGI: {PlayerStats.Instance.AGI}";
        vit.text = $"VIT: {PlayerStats.Instance.VIT}";
        tal.text = $"TAL: {PlayerStats.Instance.TAL}";
        luk.text = $"LUK: {PlayerStats.Instance.LUK}";

        critDamage.text = $"Crit DMG: {Mathf.Round(PlayerStats.Instance.CritDamage * 100)}%";
        movementSpeed.text = $"MoveSpeed: {PlayerStats.Instance.MovementSpeed:0.##}";
        healthRegen.text = $"HP Regen: {Mathf.Round(PlayerStats.Instance.HealthRegen)}";
        attackSpeed.text = $"Atk Speed: {PlayerStats.Instance.AttackSpeed:0.##}";
        cooldownReduction.text = $"CDR: {Mathf.Round(PlayerStats.Instance.CooldownReduction * 100)}%";
    }
}
