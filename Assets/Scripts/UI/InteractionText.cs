using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionText : MonoSingleton<InteractionText>
{
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Text detailText;

    public void SetActionText(string text)
    {
        actionText.text = text;
    }

    public void SetDetailText(string text)
    {
        detailText.text = text;
    }
}
