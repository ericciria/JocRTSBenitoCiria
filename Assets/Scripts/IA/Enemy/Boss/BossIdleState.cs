using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossStates
{

    public void OnEnterState(AIBoss character)
    {
        Debug.Log("Enter Idle State");
        character.agent.angularSpeed = 120;
        character.agent.speed = 3.5f;
    }
    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit Idle State");
    }
    public BossStates OnUpdate(AIBoss character)
    {
        if (character.target != null)
        {
            float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

            if (distanceToPlayer < character.pursueDistance)
            {
                return new BossPursueState();
            }
            else
            {
                character.agent.SetDestination(character.originalPosition);
            }
        }
        else
        {
            character.agent.SetDestination(character.originalPosition);
        }
        Debug.Log("Idling");
        return null;
    }
}
