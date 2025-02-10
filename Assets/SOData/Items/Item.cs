using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;  // Friendly name of the item
    public string tag;       // Unique tag or ID for the item
    public Sprite icon;      // The sprite for the UI
}