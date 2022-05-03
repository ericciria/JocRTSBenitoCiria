using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitData", menuName = "Unit Data", order = 51)]
public class UnitData : ScriptableObject
{
    [SerializeField] private string unitName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int moneyCost;
    [SerializeField] private int metalCost;
    [SerializeField] private int attackDamage;
    [SerializeField] private string typeOfUnit; 
    [SerializeField] private string damageMultiplierType; // string que indica el tipus d'unitat contra el que es mes fort
    [SerializeField] private int damageMultiplierAmount; 
    [SerializeField] private int maxHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackCooldown;
    [SerializeField] private bool constructor;

    public string UnitName
    {
        get
        {
            return unitName;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int MoneyCost
    {
        get
        {
            return moneyCost;
        }
    }
    public int MetalCost
    {
        get
        {
            return metalCost;
        }
    }

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }
    public string TypeOfUnit
    {
        get
        {
            return typeOfUnit;
        }
    }
    public string DamageMultiplierType
    {
        get
        {
            return damageMultiplierType;
        }
    }
    public int DamageMultiplierAmount
    {
        get
        {
            return damageMultiplierAmount;
        }
    }
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }
    public float AttackDistance
    {
        get
        {
            return attackDistance;
        }
    }
    public float AttackCooldown
    {
        get
        {
            return attackCooldown;
        }
    }
    public bool Constructor
    {
        get
        {
            return constructor;
        }
    }
}
