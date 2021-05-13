using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text count;

    [SerializeField]
    private TooltipTrigger tooltipTrigger;

    public void SetItem(Item item)
    {
        icon.sprite = item.icon;
        string header = item.name;
        string content = "Each stack provides:\n\n" + item.stats.GetDescription();
        tooltipTrigger.SetText(content, header);
    }

    public void SetCount(int count)
    {
        this.count.text = count.ToString();
    }
}
