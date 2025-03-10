using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerEquipment equipment;
    public PlayerStat stat;


    public ItemData itemData;
    public Action addItem;



    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equipment = GetComponent<PlayerEquipment>();
        stat = GetComponent<PlayerStat>();
    }

    public void ApplyItemEffect(ItemData item)
    {
        if (item == null || item.type != ItemType.Consumable) return;
        Debug.Log("제발 효과가 들어오게해주세요");
        foreach (var effect in item.consumables)
        {
            switch (effect.type)
            {
                case ConsumableType.Health:
                    condition.Heal(effect.value);
                    break;
                case ConsumableType.Hunger:
                    condition.Eat(effect.value);
                    break;
                case ConsumableType.Stamina:
                    condition.StaminaBoost(effect.value);
                    break;
                case ConsumableType.StatBoost:
                    stat.ApplyStatBoost(effect);
                    break;
            }
        }
    }
}