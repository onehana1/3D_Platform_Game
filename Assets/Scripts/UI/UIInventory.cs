using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;


    private PlayerController controller;
    private PlayerCondition condition;

    private bool isOpen = false;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        //dropPosition = CharacterManager.Instance.Player.dropPosition;

        InventoryManager.Instance.onInventoryUpdated += UpdateUI;


        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();  // 모든 슬롯 초기화
        }
    }

    public void Toggle()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data == null) { Debug.Log("데이터가 null"); return; }

        // InventoryManager에 아이템 추가
        InventoryManager.Instance.AddItem(data);

        // UI 업데이트
        UpdateUI();

        CharacterManager.Instance.Player.itemData = null;
    }

    public void UpdateUI()
    {
        List<ItemData> inventory = InventoryManager.Instance.GetInventory();

        // 슬롯 UI 업데이트
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                slots[i].item = inventory[i];
                slots[i].quantity = 1; // 현재 개별 아이템만 관리
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }



    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }
}
