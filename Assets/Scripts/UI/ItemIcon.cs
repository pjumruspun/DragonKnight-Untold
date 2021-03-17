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

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetCount(int count)
    {
        this.count.text = count.ToString();
    }
}
