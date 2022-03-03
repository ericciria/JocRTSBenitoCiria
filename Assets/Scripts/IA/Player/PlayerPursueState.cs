using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPursueState : PlayerUnitStates
{
    public void OnEnterState(AIPlayerunit character)
    {
        Debug.Log("Enter Pursue State");
    }

    public void OnExitState(AIPlayerunit character)
    {
        Debug.Log("Exit Pursue State");
    }

    public PlayerUnitStates OnUpdate(AIPlayerunit character)
    {
        if (character.target != null)
        {
            float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

            if (distanceToPlayer < character.attackDistance)
            {
                character.agent.SetDestination(character.transform.position);
                return new PlayerAttackState();
            }
            //Debug.Log("Pursuing");
            character.agent.SetDestination(character.target.position);
        }
        else
        {
            return new PlayerIdleState();
        }
        
        return null;
    }
}
