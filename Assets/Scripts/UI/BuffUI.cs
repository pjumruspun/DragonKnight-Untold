using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoSingleton<BuffUI>
{
    [SerializeField]
    private GameObject buffIconPrefab;
    private ObjectPool buffIconPool;

    private void Start()
    {
        buffIconPool = new ObjectPool(buffIconPrefab);
        EventPublisher.PlayerReceiveBuff += UpdateBuffDisplay;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerReceiveBuff -= UpdateBuffDisplay;
    }

    private void UpdateBuffDisplay(Buff newBuff)
    {
        GameObject newBuffIcon = buffIconPool.SpawnObject();
        if (newBuffIcon.TryGetComponent<BuffIcon>(out var buffIcon))
        {
            buffIcon.SetBuff(newBuff);
        }
        else
        {
            throw new System.InvalidOperationException("Buff icon object spawned from pool does not have BuffIcon script attached to.");
        }

        newBuffIcon.transform.SetParent(transform);
    }
}
