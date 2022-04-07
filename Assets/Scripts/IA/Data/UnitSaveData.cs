using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSaveData
{
    public float currentHealth;
    public UnitData unitData;
    public float timeSinceLastAttack;
    public int team;
    public Color teamColor;
    public Transform target;

    public UnitStates currentState;

    public float[] position = new float[3];

    public UnitSaveData(Unit unit)
    {
        currentHealth = unit.health;
        unitData = unit.unitData;
        timeSinceLastAttack = unit.timeSinceLastAttack;
        team = unit.team;
        teamColor = unit.teamColor;
        target = unit.target;

        currentState = unit.currentState;

        position = new float[3];
        position[0] = unit.transform.position.x;
        position[1] = unit.transform.position.y;
        position[2] = unit.transform.position.z;
    }
}
