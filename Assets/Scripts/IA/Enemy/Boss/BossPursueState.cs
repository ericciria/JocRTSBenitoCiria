using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPursueState : BossStates
{
    public void OnEnterState(AIBoss character)
    {
        Debug.Log("Enter Pursue State");
        character.agent.angularSpeed = 120;
        character.agent.speed = 3.5f;
    }

    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit Pursue State");
    }

    public BossStates OnUpdate(AIBoss character)
    {
        float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

        if (distanceToPlayer < character.orbitDistance)
        {
            return new BossIntroAttackState();
        }
        else if (distanceToPlayer > character.pursueDistance)
        {
            character.agent.SetDestination(character.originalPosition);
            return new BossIdleState();
        }
        Debug.Log("Pursuing");
        character.agent.SetDestination(character.target.position);
        return null;
    }
}
