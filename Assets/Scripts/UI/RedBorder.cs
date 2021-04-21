using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBorder : MonoBehaviour
{
    [SerializeField]
    private GameObject redBorderBreath;
    [SerializeField]
    private GameObject redBorderFlash;
    [SerializeField]
    private float healthThresholdToShow = 0.3f;
    private const float flashDuration = 0.2f;

    private void Start()
    {
        HideBorder();
        redBorderFlash.SetActive(false);
        EventPublisher.PlayerTakeDamage += FlashBorder;
        EventPublisher.PlayerHealthChange += ProcessBreathingRedBorder;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerTakeDamage -= FlashBorder;
        EventPublisher.PlayerHealthChange -= ProcessBreathingRedBorder;
    }

    private void FlashBorder()
    {
        redBorderFlash.SetActive(true);
        CoroutineUtility.ExecDelay(() => redBorderFlash.SetActive(false), flashDuration);
    }

    private void ProcessBreathingRedBorder()
    {
        if (PlayerHealth.Instance.HealthPercentage <= healthThresholdToShow)
        {
            ShowBorder();
        }
        else
        {
            HideBorder();
        }
    }

    private void ShowBorder()
    {
        redBorderBreath.SetActive(true);
    }

    private void HideBorder()
    {
        redBorderBreath.SetActive(false);
    }
}
