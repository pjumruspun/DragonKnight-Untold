using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkDisplayMethod
{
    Percentage,
    Number,
}

[System.Serializable]
public class PerkUpgradeDetail
{
    public string Name => upgradeName;
    public PerkDisplayMethod DisplayMethod => displayMethod;
    public float[] Values => values;

    [SerializeField]
    private string upgradeName;

    [SerializeField]
    private PerkDisplayMethod displayMethod;

    [SerializeField]
    private float[] values;
}
