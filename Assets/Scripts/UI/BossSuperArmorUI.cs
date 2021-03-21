using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSuperArmorUI : MonoBehaviour
{
    [SerializeField]
    private GameObject superArmorUI;
    [SerializeField]
    private Image superArmorFill;
    [SerializeField]
    private float delayHideAfterBossDead = 0.0f;

    private void Awake()
    {
        BossEvents.BossSpawn += ShowSAUI;
    }

    private void Start()
    {
        BossEvents.BossSAChange += UpdateSABar;
        BossEvents.BossDead += HideSAUIDelay;
    }

    private void OnDestroy()
    {
        BossEvents.BossSpawn -= ShowSAUI;
        BossEvents.BossSAChange -= UpdateSABar;
        BossEvents.BossDead -= HideSAUIDelay;
    }

    private void ShowSAUI(Boss boss)
    {
        superArmorUI.SetActive(true);
        UpdateSABar(boss);
    }

    private void UpdateSABar(Boss boss)
    {
        superArmorFill.fillAmount = boss.SuperArmorPercentage;
    }

    private void HideSAUIDelay(Boss boss)
    {
        CoroutineUtility.ExecDelay(() => superArmorUI.SetActive(false), delayHideAfterBossDead);
    }
}
