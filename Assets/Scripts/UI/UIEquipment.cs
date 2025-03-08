using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquipment : MonoBehaviour
{
    public ItemSlot headSlot;
    public ItemSlot bodySlot;
    public ItemSlot weaponSlot;

    private PlayerEquipment playerEquipment;

    private void Start()
    {
        playerEquipment = CharacterManager.Instance.Player.equipment;
    }

    public void EquipItem(ItemSlotData itemSlot)
    {
        ItemSlot targetSlot = GetSlot(itemSlot.item.equipSlotType);
        if (targetSlot == null) return;

        // 기존 장착 아이템이 있다면 인벤토리로 다시 넣기
        if (targetSlot.item != null)
        {
            InventoryManager.Instance.AddItem(targetSlot.item);
        }

        // 슬롯에 아이템 장착
        targetSlot.item = itemSlot.item;
        targetSlot.quantity = itemSlot.quantity;
        targetSlot.Set();
    }

    public void UnequipItem(EquipSlotType slotType)
    {
        ItemSlot targetSlot = GetSlot(slotType);
        if (targetSlot == null || targetSlot.item == null) return;

        InventoryManager.Instance.AddItem(targetSlot.item);
        targetSlot.Clear();
    }

    private ItemSlot GetSlot(EquipSlotType slotType)
    {
        return slotType switch
        {
            EquipSlotType.Head => headSlot,
            EquipSlotType.Body => bodySlot,
            EquipSlotType.Weapon => weaponSlot,
            _ => null
        };
    }
}
