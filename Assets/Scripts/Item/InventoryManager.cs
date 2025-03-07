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


    [SerializeField] private List<ItemData> inventory = new List<ItemData>();
    public event Action onInventoryUpdated; // �κ��� �� ������ ui�߰����ִ� �̺�Ʈ �߰�

    public void AddItem(ItemData item)
    {
        inventory.Add(item);
        Debug.Log($"������ �߰���: {item.displayName}");
        onInventoryUpdated?.Invoke();
    }

    public List<ItemData> GetInventory()
    {
        return inventory;
    }


}
