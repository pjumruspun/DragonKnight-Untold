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
    public T GetInt => (T)(((dynamic)baseValue + Additive) * (1.0f + Multiplicative));

    /// <summary>
    /// Additive bonus of this stat
    /// Default is 0
    /// </summary>
    /// <value></value>
    public T Additive { get; set; }

    /// <summary>
    /// Multiplicative bonus of this stat
    /// Default is 0.0f
    /// </summary>
    /// <value></value>
    public float Multiplicative { get; set; }

    /// <summary>
    /// Base stat without any increment
    /// </summary>
    [SerializeField]
    private T baseValue;

    public Stats(T baseValue)
    {
        this.baseValue = baseValue;
    }
}
