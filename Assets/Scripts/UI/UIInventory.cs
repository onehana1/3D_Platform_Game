using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Item Tooltip")]
    public GameObject itemTooltip; // 아이템 설명 박스
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public GameObject useButton;
    public GameObject EquipUseButton;
    public GameObject dropButton;

    private ItemSlot selectedItem;   // 이게 선택된 아이ㅌ
    private int selectedItemIndex;

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
        itemTooltip.SetActive(false);

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
        bool isOpening = !inventoryWindow.activeSelf;
        inventoryWindow.SetActive(isOpening);

        if (!isOpening)
        {
            HideItemTooltip();
        }
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
        List<ItemSlotData> inventory = InventoryManager.Instance.GetInventory();

        // 슬롯 UI 업데이트
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                slots[i].item = inventory[i].item;
                slots[i].quantity = inventory[i].quantity;
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

    public void ShowItemTooltip(ItemSlot slot)
    {
        itemTooltip.SetActive(true);
        itemTooltip.transform.position = slot.transform.position + new Vector3(75, -75, 0); // 슬롯 옆으로 위치 조정

        itemNameText.text = slot.item.displayName;
        itemDescriptionText.text = slot.item.description;
        itemStatsText.text = "";

        foreach (var consumable in slot.item.consumables)
        {
            itemStatsText.text += $"{consumable.type}: {consumable.value}\n";
        }
    }

    public void HideItemTooltip()
    {
        itemTooltip.SetActive(false);
    }


    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        // 전에 선택한게 잇으면 그거 아웃라인 없애고 그다음에 아웃라인 넣기
        if (selectedItem != null)
        {
            selectedItem.SetOutline(false);
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItem.SetOutline(true);

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        EquipUseButton.SetActive(selectedItem.item.type == ItemType.Equipable);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem == null || selectedItem.item == null) return;

        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value); break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumables[i].value); break;
                }
            }
            InventoryManager.Instance.UseItem(selectedItem.item);
            UpdateUI();


        }
    }

    public void OnThrowButton()
    {
        if (selectedItem == null || selectedItem.item == null) return;

        Vector3 dropPosition = CharacterManager.Instance.Player.transform.position + CharacterManager.Instance.Player.transform.forward * 1.5f;
        InventoryManager.Instance.ThrowItem(selectedItem.item, dropPosition);

        UpdateUI();

    }

    public void OnEquipButton()
    {
        if (selectedItem == null || selectedItem.item.type != ItemType.Equipable) return;

        UIEquipment equipmentUI = FindObjectOfType<UIEquipment>();
        equipmentUI.EquipItem(new ItemSlotData(selectedItem.item, selectedItem.quantity));
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
               
            }

            selectedItem.item = null;
        }

        UpdateUI();
    }

}
