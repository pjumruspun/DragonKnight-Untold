using UnityEngine;

[System.Serializable]
public class Stats<T>
{
    public T GetValue => baseValue;
    [SerializeField]
    private T baseValue;

    public Stats(T baseValue)
    {
        this.baseValue = baseValue;
    }
}
