using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleWarState : BossStates
{
    public void OnEnterState(AIBoss character)
    {
        Debug.Log("Enter IdleWar State");
        character.randomDir = Random.Range(-1,2);
        while (character.randomDir == 0)
        {
            character.randomDir = Random.Range(-1, 2);
        }
        character.agent.angularSpeed = 0;
        character.agent.speed = 0;

    }

    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit IdleWar State");
    }

    public BossStates OnUpdate(AIBoss character)
    {
        character.randomInt = Random.Range(0, 3);


        if (character.target != null)
        {
            if (Vector3.Distance(character.transform.position, character.target.transform.position) <= character.attackDistance - 2)
            {
                character.transform.position += -character.transform.forward * Time.deltaTime * 5;
            }
            else if (Vector3.Distance(character.transform.position, character.target.transform.position) <= character.orbitDistance)
            {
                if (character.randomInt < 2)
                {
                    return new BossAttackState();
                }
                else
                {
                    return new BossIntroAttackState();
                }
            }
            else
            {
                return new BossPursueState();
            }
        }
        else
        {
            return new BossIdleState();
        }
        
        return null;
    }
}

