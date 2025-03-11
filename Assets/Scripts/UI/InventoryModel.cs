using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    public event Action OnInventoryUpdated;
    private List<ItemSlotData> inventoryData = new List<ItemSlotData>();

    public List<ItemSlotData> GetInventory() => inventoryData;

    public void AddItem(ItemData item)
    {
        ItemSlotData existingSlot = inventoryData.Find(slot => slot.item == item && slot.quantity < item.maxStackAmount);
        if (existingSlot != null)
        {
            existingSlot.quantity++;
        }
        else
        {
            inventoryData.Add(new ItemSlotData(item, 1));
        }

        OnInventoryUpdated?.Invoke();
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= inventoryData.Count) return;

        inventoryData.RemoveAt(index);
        OnInventoryUpdated?.Invoke();
    }
}
