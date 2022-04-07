using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConstructState : UnitStates
{
    Building building;
    void UnitStates.OnEnterState(Unit unit)
    {
        Debug.Log("Enter Construct State");
        building = unit.target.gameObject.GetComponentInParent<Building>();
        Debug.Log(building);
        unit.agent.SetDestination(unit.transform.position);
    }

    void UnitStates.OnExitState(Unit unit)
    {
        //Debug.Log("Exit Construct State");
    }

    UnitStates UnitStates.OnUpdate(Unit unit)
    {
        if (unit.target != null && !building.constructed)
        {
            building.Construct();
        }
        else
        {
            return new UnitIdleState();
        }
        return null;
    }
}
