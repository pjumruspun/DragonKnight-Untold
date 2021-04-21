using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashEffectSize : MonoBehaviour
{
    private Animator animator;
    private const float smallestSize = 0.6f;
    private const float biggestSize = 1.0f;
    private float originalX;
    private float originalY;

    public void SetSize(int chargeLevel, int maxChargeLevel)
    {
        float sizePerLevel = (biggestSize - smallestSize) / (maxChargeLevel - 1);
        SetSize(smallestSize + ((chargeLevel - 1) * sizePerLevel));
    }

    private void SetSize(float size)
    {
        transform.localScale = new Vector3(originalX * size, originalY * size, 1.0f);
    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(originalX, originalY, 1.0f);
    }

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        originalX = transform.localScale.x;
        originalY = transform.localScale.y;
    }
}
