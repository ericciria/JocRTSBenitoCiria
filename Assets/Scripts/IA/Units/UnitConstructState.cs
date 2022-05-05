using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConstructState : UnitStates
{
    Building building;
    void UnitStates.OnEnterState(Unit unit)
    {
        unit.anim.SetBool("isBuilding", true);
        Debug.Log("Enter Construct State");
        building = unit.target.gameObject.GetComponentInParent<Building>();
        unit.agent.SetDestination(unit.transform.position);
    }

    void UnitStates.OnExitState(Unit unit)
    {
        unit.anim.SetBool("isBuilding", false);
        //Debug.Log("Exit Construct State");
    }

    UnitStates UnitStates.OnUpdate(Unit unit)
    {
        

        if (unit.target != null && !building.constructed)
        {
            building.Construct();

            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.position - unit.transform.position), 5 * Time.deltaTime);
            
        }
        else
        {
            return new UnitIdleState();
        }
        return null;
    }
}
