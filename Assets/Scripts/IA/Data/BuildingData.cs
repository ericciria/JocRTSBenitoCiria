using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingData", menuName = "Building Data", order = 52)]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string buildingName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int moneyCost;
    [SerializeField] private int metalCost;
    [SerializeField] private int energyProductionConsumption;
    [SerializeField] private int attackDamage; //nomes cuan es una torreta
    [SerializeField] private int maxHealth;

    public string BuildingName
    {
        get
        {
            return buildingName;
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
    public int EnergyCost
    {
        get
        {
            return energyProductionConsumption;
        }
    }

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
}
