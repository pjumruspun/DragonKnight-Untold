using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoSingleton<BossHealthUI>
{
    [SerializeField]
    private bool showHpPercentage = true;
    [SerializeField]
    private GameObject healthUIPanel;
    [SerializeField]
    private Image healthFill;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private float delayHideAfterBossDead = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        BossEvents.BossSpawn += ShowHpUI;
    }

    private void Start()
    {
        BossEvents.BossHpChange += UpdateHPBar;
        BossEvents.BossHpChange += UpdateHPText;
        BossEvents.BossDead += HideHpUIDelay;
        hpText.gameObject.SetActive(showHpPercentage);
    }

    private void OnDestroy()
    {
        BossEvents.BossHpChange -= UpdateHPBar;
        BossEvents.BossHpChange -= UpdateHPText;
        BossEvents.BossSpawn -= ShowHpUI;
        BossEvents.BossDead -= HideHpUIDelay;
    }

    private void UpdateUI(Boss boss)
    {
        UpdateHPBar(boss);
        UpdateHPText(boss);
    }

    private void UpdateHPBar(Boss boss)
    {
        healthFill.fillAmount = boss.HealthPercentage;
    }

    private void UpdateHPText(Boss boss)
    {
        hpText.text = $"{Mathf.Ceil(boss.HealthPercentage * 100)}%";
    }

    private void ShowHpUI(Boss boss)
    {
        healthUIPanel.SetActive(true);
        UpdateUI(boss);
    }

    private void HideHpUIDelay(Boss boss)
    {
        CoroutineUtility.ExecDelay(() => healthUIPanel.SetActive(false), delayHideAfterBossDead);
    }
}
