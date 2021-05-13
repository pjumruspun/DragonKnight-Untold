using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoSingleton<TooltipManager>
{
    [SerializeField]
    private Tooltip tooltip;

    public static void Show(string content, string header = "")
    {
        Instance.tooltip.SetText(content, header);
        Instance.tooltip.gameObject.SetActive(true);
        Debug.Log($"Showing content: {content}");
    }

    public static void Hide()
    {
        Instance.tooltip.gameObject.SetActive(false);
    }

    private void Start()
    {
        tooltip.gameObject.SetActive(false);
    }
}
