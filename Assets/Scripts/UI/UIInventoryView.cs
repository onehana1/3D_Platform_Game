using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryView : MonoBehaviour
{
    public event Action<int> OnItemClicked;

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private Transform slotPanel;
    [SerializeField] private GameObject itemTooltip;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemStatsText;

    private ItemSlot[] slots;

    private void Awake()
    {
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].OnClick += HandleItemClicked; // 클릭 이벤트 연결
        }
        inventoryWindow.SetActive(false);
        itemTooltip.SetActive(false);
    }

    public void ToggleInventory()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);
        if (!inventoryWindow.activeSelf)
            HideItemTooltip();
    }

    public void UpdateInventoryUI(List<ItemSlotData> inventoryData)
    {
        Debug.Log("인벤 업데이트합니다.");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryData.Count)
            {
                slots[i].Set(inventoryData[i].item, inventoryData[i].quantity, inventoryData[i].isEquipped);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    private void HandleItemClicked(int index)
    {
        OnItemClicked?.Invoke(index);
    }

    public void HideItemTooltip()
    {
        itemTooltip.SetActive(false);
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemStatsText.text = "";
    }
}
