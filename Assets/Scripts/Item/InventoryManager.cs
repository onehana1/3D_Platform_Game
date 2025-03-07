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
    public event Action onInventoryUpdated; // 인벤에 뭐 들어오면 ui추가해주는 이벤트 추가

    public void AddItem(ItemData item)
    {
        inventory.Add(item);
        Debug.Log($"아이템 추가됨: {item.displayName}");
        onInventoryUpdated?.Invoke();
    }

    public List<ItemData> GetInventory()
    {
        return inventory;
    }


}
