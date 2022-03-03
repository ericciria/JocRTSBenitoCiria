using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossStates
{
    public void OnEnterState(AIBoss character)
    {
        Debug.Log("Enter Boss Attack State");
        character.randomInt = Random.Range(0, 4);
        character.agent.angularSpeed = 0;
        character.agent.speed = 0;

        character.m_currentWaitTime = 0f;
        character.m_waitTime = 1f;

        character.currentCombo = 0;
    }

    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit Boss Attack State");
    }

    public BossStates OnUpdate(AIBoss character)
    {
        float distanceToPlayer;
        
        if (character.target != null)
        {
            distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);
        }
        else
        {
            return new BossIdleState();
        }

        if (character.timeSinceLastAttack > character.attackCooldown)
        {
            // Attack1 - Atac a distància
            if (character.randomInt == 0)
            {
                Vector3 directionToTarget = character.transform.position - character.target.position;
                float angle = Vector3.Angle(character.transform.forward, directionToTarget);
                //Debug.Log(Mathf.Abs(angle));

                if (distanceToPlayer > character.attackDistance)
                {
                    character.transform.LookAt(character.target.position);
                    character.transform.position += (character.target.position - character.transform.position).normalized * 4 * Time.deltaTime;
                }
                else if (Mathf.Abs(angle) > 175)
                {
                    if (character.timeSinceLastAttack > character.attackCooldown)
                    {
                        Debug.Log("Boss Attack1");
                        character.maxCombo = 1;
                        return new BossAttackComboState();
                    }
                }
            }

            // Attack2 - Atac Cos a Cos
            else if (character.randomInt == 1 || character.randomInt == 2)
            {
                Vector3 directionToTarget = character.transform.position - character.target.position;
                float angle = Vector3.Angle(character.transform.forward, directionToTarget);
                character.transform.LookAt(character.target.position);

                if (distanceToPlayer > character.orbitDistance)
                {
                    return new BossIdleWarState();
                }
                else if (distanceToPlayer > 2f)
                {
                    character.transform.position += (character.target.position - character.transform.position).normalized * 4 * Time.deltaTime;
                }
                else if (character.timeSinceLastAttack > character.attackCooldown)
                {
                    //character.agent.SetDestination(character.transform.position);
                    character.randomInt = Random.Range(0, 3);

                    //Combo1
                    if (character.randomInt == 0)
                    {
                        Debug.Log("Boss Attack2 Combo1");
                        character.maxCombo = 2;
                        return new BossAttackComboState();
                    }
                    //Combo2
                    if (character.randomInt == 1)
                    {
                        Debug.Log("Boss Attack2 Combo2");
                        character.maxCombo = 3;
                        return new BossAttackComboState();
                    }
                    //AtacEspecial
                    if (character.randomInt == 2)
                    {
                        Debug.Log("Boss Attack Special");
                        character.GetComponent<ParticleSystem>().Play();
                        character.target.gameObject.GetComponent<ObjectLife>().takeDamage(character.attack * 2);
                        character.timeSinceLastAttack = 0;
                        return new BossIdleWarState();
                    }
                }
            }
            // Retorna al punt inicial
            else
            {
                return new BossIdleState();
            }
        }
        else
        {
            return new BossIdleWarState();
        }

        return null;
    }
}
