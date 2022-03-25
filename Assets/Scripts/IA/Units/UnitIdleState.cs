using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitStates
{
    void UnitStates.OnEnterState(Unit unit)
    {
        unit.target = null;
        //Debug.Log("Enter Idle State");
    }

    void UnitStates.OnExitState(Unit unit)
    {
        //Debug.Log("Exit Idle State");
    }

    UnitStates UnitStates.OnUpdate(Unit unit)
    {
        if (unit.target != null)
        {
            return new UnitPursueState();
        }
        else if (unit.nearestEnemy != null && !unit.constructor)
        {
            float distanceToTarget = Vector3.Distance(unit.nearestEnemy.transform.position, unit.transform.position);
            if (distanceToTarget < unit.attackDistance)
            {
                //Debug.LogWarning(distanceToTarget);
                //Debug.LogWarning(unit.attackDistance);
                return new UnitAttackState();
            }
        }

        //Debug.Log("Idling");
        //Debug.Log(distanceToPlayer);
        //Debug.Log(character.pursueDistance);
        return null;
    }
}
