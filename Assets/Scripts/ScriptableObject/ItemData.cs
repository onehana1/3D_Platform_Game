using System;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health,
    Stamina,
    StatBoost
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;

    public StatType statType; 
    public bool isTemporary;  // 일시적?
    public float duration; // 지속 시간 
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equipable")]
    public EquipSlotType equipSlotType;
    public GameObject equipPrefabs;

    public float attackBonus;
    public float defenseBonus;
    public float moveSpeedBonus;
    public float jumpPowerBonus;
}


[Serializable] 
public class ItemSlotData  // 아이템 데이터로는 개수 못하니까 아이템 구조 추가
{
    public ItemData item;
    public int quantity;
    public bool isEquipped; // 장착함?

    public ItemSlotData(ItemData item, int quantity = 1)
    {
        this.item = item;
        this.quantity = quantity;
        isEquipped = false;
    }
    public ItemSlotData() { }
}

