using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerUnitStates
{
    public void OnEnterState(AIPlayerunit character)
    {
        Debug.Log("Enter Idle State");
    }

    public void OnExitState(AIPlayerunit character)
    {
        Debug.Log("Exit Idle State");
    }

    public PlayerUnitStates OnUpdate(AIPlayerunit character)
    {
        if (character.target != null)
        {
            return new PlayerPursueState();
        }
        else if (character.nearestEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(character.nearestEnemy.transform.position, character.transform.position);
            if (distanceToTarget < character.attackDistance)
            {
                Debug.LogWarning(distanceToTarget);
                Debug.LogWarning(character.attackDistance);
                return new PlayerAttackState();
            }
        }

        //Debug.Log("Idling");
        //Debug.Log(distanceToPlayer);
        //Debug.Log(character.pursueDistance);
        return null;
    }
}
