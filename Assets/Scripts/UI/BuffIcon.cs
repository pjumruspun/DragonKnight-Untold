using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField]
    private Image buffIcon;
    [SerializeField]
    private Image buffMask;
    private Buff buff;

    public void SetBuff(Buff buff)
    {
        this.buff = buff;
        // Set icon here later
    }

    private void Update()
    {
        buffMask.fillAmount = 1.0f - buff.DurationRatio;
        if (buff.IsExpired)
        {
            // Remove itself from parent and set to inactive
            transform.SetParent(null);
            gameObject.SetActive(false);
        }
    }
}
