using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipSlotType
{
    Head, 
    Body, 
    Weapon
}

public class PlayerEquipment : MonoBehaviour
{
    public event Action <PlayerEquipment> OnPlayerEquipment;

    public ItemData equipHead;
    public ItemData equipBody;
    public ItemData equipWeapon;


    public void EquipItem(ItemData item)
    {
        if (item == null || item.type != ItemType.Equipable) return;

        // ���� ������ �������� �����ϰ� ���ο� ������ ����
        switch (item.equipSlotType)
        {
            case EquipSlotType.Head:
                UnequipItem(equipHead);
                equipHead = item;
                break;
            case EquipSlotType.Body:
                UnequipItem(equipBody);
                equipBody = item;
                break;
            case EquipSlotType.Weapon:
                UnequipItem(equipWeapon);
                equipWeapon = item;
                break;
        }

        InventoryManager.Instance.SetEquippedState(item, true);
        FindObjectOfType<UIEquipment>()?.EquipItem(new ItemSlotData(item, 1));
    }

    public void UnequipItem(ItemData item)
    {
        if (item == null) return;

        // ������ ������ ����
        switch (item.equipSlotType)
        {
            case EquipSlotType.Head:
                equipHead = null;
                break;
            case EquipSlotType.Body:
                equipBody = null;
                break;
            case EquipSlotType.Weapon:
                equipWeapon = null;
                break;
        }
        InventoryManager.Instance.SetEquippedState(item, false);
        FindObjectOfType<UIEquipment>()?.UnequipItem(item.equipSlotType);
    }
}



