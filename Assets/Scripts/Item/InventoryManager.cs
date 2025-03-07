using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private PlayerController controller;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    [SerializeField] private List<ItemSlotData> inventory = new List<ItemSlotData>();
    public event Action onInventoryUpdated; // �κ��� �� ������ ui�߰����ִ� �̺�Ʈ �߰�

    public void AddItem(ItemData newItem)
    {
        if (newItem == null) return;

        if (newItem.canStack)
        {
            ItemSlotData existingSlot = inventory.Find(slot => slot.item == newItem);
            if (existingSlot != null && existingSlot.quantity < newItem.maxStackAmount)
            {
                existingSlot.quantity++;
                onInventoryUpdated?.Invoke();
                Debug.Log($"{newItem.displayName} ���� ����: {existingSlot.quantity}");
                return;
            }
        }

        inventory.Add(new ItemSlotData(newItem, 1));
        onInventoryUpdated?.Invoke();
        Debug.Log($"�� ������ �߰�: {newItem.displayName}");
    }

    public List<ItemSlotData> GetInventory()
    {
        return inventory;
    }


}

