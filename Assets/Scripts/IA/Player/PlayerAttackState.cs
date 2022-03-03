using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerUnitStates
{
    

    public void OnEnterState(AIPlayerunit character)
    {
        Debug.Log("Enter Attack State");
    }

    public void OnExitState(AIPlayerunit character)
    {
        Debug.Log("Exit Attack State");
    }

    public PlayerUnitStates OnUpdate(AIPlayerunit character)
    {
        Vector3 directionToTarget;
        float angle;
        float distanceToPlayer;
        if (character.target != null)
        {
            distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);
            directionToTarget = character.transform.position - character.target.position;
            angle = Vector3.Angle(character.transform.forward, directionToTarget);
            //Debug.Log(Mathf.Abs(angle));

            if (distanceToPlayer > character.attackDistance)
            {
                return new PlayerPursueState();
            }
            else if (Mathf.Abs(angle) > 175)
            {
                if (character.timeSinceLastAttack > character.attackCooldown)
                {
                    character.GetComponent<ParticleSystem>().Play();
                    Debug.Log("Attack");
                    character.target.gameObject.GetComponent<ObjectLife>().takeDamage(character.attack);
                    character.timeSinceLastAttack = 0;
                }
                //Debug.Log("Target in front of unit");
            }
            else
            {
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(character.target.position - character.transform.position), 5 * Time.deltaTime);
            }
        }
        else if (character.nearestEnemy != null)
        {
            distanceToPlayer = Vector3.Distance(character.nearestEnemy.transform.position, character.transform.position);
            if (distanceToPlayer < character.attackDistance)
            {
                directionToTarget = character.transform.position - character.nearestEnemy.transform.position;
                angle = Vector3.Angle(character.transform.forward, directionToTarget);
                if (Mathf.Abs(angle) > 175)
                {
                    if (character.timeSinceLastAttack > character.attackCooldown)
                    {
                        character.GetComponent<ParticleSystem>().Play();
                        Debug.Log("Attack");
                        character.nearestEnemy.GetComponent<ObjectLife>().takeDamage(character.attack);
                        character.timeSinceLastAttack = 0;
                    }
                    //Debug.Log("Target in front of unit");
                }
                else
                {
                    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(character.nearestEnemy.transform.position - character.transform.position), 5 * Time.deltaTime);
                }
            }
            else
            {
                return new PlayerIdleState();
            }
        }
        else
        {
            return new PlayerIdleState();
        }

        return null;
    }
}
