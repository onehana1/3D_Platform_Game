using System;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditor.UIElements;
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

    public void UseItem(ItemData selectedItem)
    {
        if (selectedItem == null) return;
        ItemSlotData item = inventory.Find(slot => slot.item== selectedItem);
        if(item== null) return;
        item.quantity--;
        Debug.Log($"{selectedItem.displayName} ����߼�");

        if (item.quantity <= 0)
        {
            inventory.Remove(item);
        }
        onInventoryUpdated?.Invoke();

    }


    public void ThrowItem(ItemData selectedItem, Vector3 dropPos)
    {
        if(selectedItem==null) return;
        ItemSlotData item = inventory.Find(slot => slot.item == selectedItem);

        if(item==null) return;

        item.quantity--;
        if(item.quantity <= 0) inventory.Remove(item);

        GameObject droppedItem = Instantiate(item.item.dropPrefab, dropPos, Quaternion.identity);
        droppedItem.GetComponent<ItemObject>().data = item.item;

        onInventoryUpdated?.Invoke();

    }

    public List<ItemSlotData> GetInventory()
    {
        return inventory;
    }


}

