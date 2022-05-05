using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPursueState : UnitStates
{
    void UnitStates.OnEnterState(Unit unit)
    {

        //Debug.Log("Enter Pursue State");
        if (unit.anim != null)
        {
            unit.anim.SetBool("walk", true);
        }
        
        if (unit.constructor)
        {
            unit.agent.SetDestination(unit.target.position);
        }
    }

    void UnitStates.OnExitState(Unit unit)
    {
        //Debug.Log("Exit Pursue State");
        if (unit.anim != null)
        {
            unit.anim.SetBool("walk", false);
        }
    }

    UnitStates UnitStates.OnUpdate(Unit unit)
    {
        if (unit.target != null)
        {
            float distanceToPlayer = Vector3.Distance(unit.target.position, unit.transform.position);
            if (!unit.constructor)
            {
                if (distanceToPlayer < unit.attackDistance)
                {
                    unit.agent.SetDestination(unit.transform.position);
                    return new UnitAttackState();
                }
                //Debug.Log("Pursuing");
                unit.agent.SetDestination(unit.target.position);
            }
            else
            {
                if (distanceToPlayer < 4)
                {
                    return new UnitConstructState();
                }
            }
            
        }
        else
        {
            return new UnitIdleState();
        }

        return null;
    }
}
