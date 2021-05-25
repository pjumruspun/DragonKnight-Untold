using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private string header = default;

    [SerializeField]
    [TextArea]
    private string content = default;

    public void SetText(string content, string header = "")
    {
        this.header = header;
        this.content = content;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }
}
