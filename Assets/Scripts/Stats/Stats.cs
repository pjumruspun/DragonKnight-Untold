using UnityEngine;

/// <summary>
/// Stats that can be dynamically increased, supports both
/// additive and multiplicative increment.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class Stats<T>
{
    /// <summary>
    /// Real stats
    /// (baseValue + Additive) * (1.0f + Multiplicative)
    /// </summary>
    public T GetValue => actualValue;

    /// <summary>
    /// Additive bonus of this stat
    /// Default is 0
    /// </summary>
    /// <value></value>
    public T Additive
    {
        set
        {
            additive = value;
            UpdateActualValue();
        }
    }

    /// <summary>
    /// Multiplicative bonus of this stat
    /// Default is 0.0f
    /// </summary>
    /// <value></value>
    public float Multiplicative
    {
        set
        {
            multiplicative = value;
            UpdateActualValue();
        }
    }

    [SerializeField]
    private T actualValue;
    private T additive;
    private float multiplicative;
    private T baseValue;

    public Stats(T baseValue)
    {
        this.baseValue = baseValue;
        actualValue = (T)(((dynamic)baseValue + additive) * (1.0f + multiplicative));
    }

    private void UpdateActualValue()
    {
        actualValue = (T)(((dynamic)baseValue + additive) * (1.0f + multiplicative));
    }
}
