using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntroAttackState : BossStates
{
    public void OnEnterState(AIBoss character)
    {
        character.randomInt = Random.Range(2, 4);
        //character.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        Debug.Log("Enter IntroAttack State");
        character.agent.angularSpeed = 0;
        character.agent.speed = 0;

        character.m_currentWaitTime = 0f;
        character.m_waitTime = character.randomInt;
    }

    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit IntroAttack State");
    }

    public BossStates OnUpdate(AIBoss character)
    {
        if (character.target != null)
        {
            if (Vector3.Distance(character.transform.position, character.target.transform.position) <= character.orbitDistance)
            {

                character.transform.RotateAround(character.target.position, Vector3.up, character.orbitDistance * Time.deltaTime);
                character.transform.LookAt(character.target);

            }
            else
            {
                character.agent.angularSpeed = 120;
                character.agent.speed = 3.5f;
                return new BossPursueState();
            }

            if (character.m_currentWaitTime > character.m_waitTime)
            {
                character.m_currentWaitTime = 0;
                return new BossIdleWarState();
            }
            character.m_currentWaitTime += Time.deltaTime;
        }
        else
        {
            return new BossIdleState();
        }
        

        return null;
    }
}
