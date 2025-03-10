using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float moveSpeed = 5.0f;   // 기본 이동 속도
    [SerializeField] private float jumpPower = 100.0f;  // 점프력
    [SerializeField] private float attackPower = 5.0f; // 공격력
    [SerializeField] private float defensePower = 2.0f; // 방어력

    public float MoveSpeed => moveSpeed;
    public float JumpPower => jumpPower;
    public float AttackPower => attackPower;
    public float DefensePower => defensePower;

    public void ModifyStat(StatType stat, float amount)
    {
        switch (stat)
        {
            case StatType.MoveSpeed:
                moveSpeed = Mathf.Max(0, moveSpeed + amount);
                break;
            case StatType.JumpPower:
                jumpPower = Mathf.Max(0, jumpPower + amount);
                break;
            case StatType.AttackPower:
                attackPower = Mathf.Max(0, attackPower + amount);
                break;
            case StatType.DefensePower:
                defensePower = Mathf.Max(0, defensePower + amount);
                break;
        }

    }

    public float GetStatValue(StatType stat)
    {
        return stat switch
        {
            StatType.MoveSpeed => moveSpeed,
            StatType.JumpPower => jumpPower,
            StatType.AttackPower => attackPower,
            StatType.DefensePower => defensePower,
            _ => 0f
        };
    }


    public void ApplyStatBoost(ItemDataConsumable effect)
    {
        if (effect.isTemporary)
        {
            StartCoroutine(TemporaryStatBoost(effect.statType, effect.value, effect.duration));
        }
        else
        {
            ModifyStat(effect.statType, effect.value);
        }
    }

    private IEnumerator TemporaryStatBoost(StatType stat, float value, float duration)
    {
        ModifyStat(stat, value);
        yield return new WaitForSeconds(duration);
        ModifyStat(stat, -value);
    }
}

public enum StatType
{
    MoveSpeed,
    JumpPower,
    AttackPower,
    DefensePower,
    None
}

