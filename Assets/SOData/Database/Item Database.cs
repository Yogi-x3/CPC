using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items;

    private Dictionary<string, Item> itemDict;

    private void OnEnable()
    {
        itemDict = new Dictionary<string, Item>();
        foreach (Item item in items)
        {
            itemDict[item.tag] = item;
        }
    }

    public Item GetItemByTag(string tag)
    {
        itemDict.TryGetValue(tag, out Item item);
        return item;
    }
}
