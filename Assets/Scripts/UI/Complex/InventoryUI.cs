using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public Image[] inventorySlots;
    public int nextAvailableSlot = 0;

    void Start()
    {
        InitializeInventorySlots();
    }

    public void AddItemToUI(string itemTag, int slotIndex)
    {
        Item item = itemDatabase.GetItemByTag(itemTag);
        if (item == null)
        {
            Debug.LogError($"Item with tag '{itemTag}' not found in database.");
            return;
        }

        // Assign the item's sprite to the specified slot and enable it
        inventorySlots[slotIndex].sprite = item.icon;
        inventorySlots[slotIndex].enabled = true; // Enable the slot to show the item
    }

    private void InitializeInventorySlots()
    {
        // Loop through each slot and disable its Image component
        foreach (Image slot in inventorySlots)
        {
            slot.enabled = false;  // Disable the image component to mark the slot as empty
            slot.sprite = null;    // Optional: clear any assigned sprite to ensure it's truly empty
        }
    }

    //public void RemoveItemFromUI(int slotIndex)
    //{
    //    if (slotIndex < 0 || slotIndex >= inventorySlots.Length)
    //    {
    //        Debug.LogWarning("Invalid slot index. Cannot remove item.");
    //        return;
    //    }

    //    // Clear the sprite and disable the Image component to mark it as empty
    //    inventorySlots[slotIndex].sprite = null;
    //    inventorySlots[slotIndex].enabled = false; // Mark the slot as empty

    //    // Optional: Update next available slot index
    //    nextAvailableSlot = FindNextAvailableSlot();
    //}
}
