using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoSingleton<ItemUI>
{
    [SerializeField]
    private GameObject itemDisplayPrefab;
    private Dictionary<Item, GameObject> itemIcons = new Dictionary<Item, GameObject>();

    private void Start()
    {
        EventPublisher.InventoryChange += UpdateItemDisplay;
    }

    private void OnDestroy()
    {
        EventPublisher.InventoryChange -= UpdateItemDisplay;
    }

    private void UpdateItemDisplay()
    {
        foreach (var itemCount in Inventory.Instance.ItemCount)
        {
            Item item = itemCount.Key;
            int count = itemCount.Value;
            if (itemIcons.TryGetValue(item, out GameObject itemIconObj))
            {
                // Exist
                if (itemIconObj.TryGetComponent<ItemIcon>(out ItemIcon itemIcon))
                {
                    itemIcon.SetCount(count);
                }
                else
                {
                    throw new System.NullReferenceException("Existed item icon obj does not contain ItemIcon");
                }
            }
            else
            {
                // Not exist, instantiate new icon
                GameObject icon = Instantiate(itemDisplayPrefab);
                if (icon.TryGetComponent<ItemIcon>(out ItemIcon itemIcon))
                {
                    itemIcon.SetIcon(item.icon);
                    itemIcon.SetCount(count);
                    icon.transform.parent = transform;
                    itemIcons[item] = icon;
                }
                else
                {
                    throw new System.NullReferenceException("Prefab does not contain ItemIcon");
                }
            }
        }
    }
}
