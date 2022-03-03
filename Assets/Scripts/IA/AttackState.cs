using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    /*
     *  Test Enemy
     */

    private float timeSinceLastAttack = Mathf.Infinity;

    public void OnEnterState(AICharacter character)
    {
        Debug.Log("Enter Attack State");
    }
    public void OnExitState(AICharacter character)
    {
        Debug.Log("Exit Attack State");
    }
    public IState OnUpdate(AICharacter character)
    {
        timeSinceLastAttack += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);
    
        if (distanceToPlayer > character.atackDistance)
        {
            return new PursueState();
        }
        if(timeSinceLastAttack > character.atackCooldown)
        {
            Debug.Log("Attack");
            timeSinceLastAttack = 0;
        }
        return null;
    }

    /*
     *  Patrolling Enemy
     */
    void IState.OnEnterState(PatrollingEnemy character)
    {
        Debug.Log("Enter Attack State");
    }
    void IState.OnExitState(PatrollingEnemy character)
    {
        Debug.Log("Exit Attack State");
    }
    IState IState.OnUpdate(PatrollingEnemy character)
    {
        timeSinceLastAttack += Time.deltaTime;
        if (character.target != null)
        {
            float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

            Vector3 directionToTarget = character.transform.position - character.target.position;
            float angle = Vector3.Angle(character.transform.forward, directionToTarget);

            if (distanceToPlayer > character.atackDistance)
            {
                return new IdleState();     // Torna a patrullar
            }
            if (timeSinceLastAttack > character.atackCooldown)
            {
                if (Mathf.Abs(angle) > 175)
                {
                    if (timeSinceLastAttack > character.atackCooldown)
                    {
                        character.GetComponent<ParticleSystem>().Play();
                        Debug.Log("Attack");
                        character.target.gameObject.GetComponent<ObjectLife>().takeDamage(character.attack);
                        timeSinceLastAttack = 0;
                    }
                    Debug.Log("Target in front of unit");
                }
                else
                {
                    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(character.target.position - character.transform.position), 1 * Time.deltaTime);
                }
            }
        }
        else
        {
            return new IdleState();
        }
        
        return null;
    }

    /*
     * Turret
     */
    public void OnEnterState(AITurret character)
    {
        Debug.Log("Enter Idle State");
        character.target = character.nearestEnemy.transform;
    }
    public void OnExitState(AITurret character)
    {
        Debug.Log("Exit Idle State");
    }
    public IState OnUpdate(AITurret character)
    {

        timeSinceLastAttack += Time.deltaTime;
        if (character.target != null)
        {
            float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

            Vector3 directionToTarget = character.transform.position - character.target.position;
            float angle = Vector3.Angle(character.transform.forward, directionToTarget);

            if (distanceToPlayer > character.attackDistance)
            {
                return new IdleState();     // Torna a patrullar
            }
            if (timeSinceLastAttack > character.attackCooldown)
            {
                if (Mathf.Abs(angle) > 175)
                {
                    if (timeSinceLastAttack > character.attackCooldown)
                    {
                        character.GetComponent<ParticleSystem>().Play();
                        Debug.Log("Attack");
                        character.target.gameObject.GetComponent<ObjectLife>().takeDamage(character.attack);
                        timeSinceLastAttack = 0;
                    }
                    Debug.Log("Target in front of unit");
                }
                else
                {
                    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(character.target.position - character.transform.position), 2 * Time.deltaTime);
                }
            }
        }
        else
        {
            return new IdleState();
        }

        return null;
    }

    /*
     * NormalEnemy
     */
    public void OnEnterState(AIEnemyUnit character)
    {
        Debug.Log("Enter Idle State");
        
    }
    public void OnExitState(AIEnemyUnit character)
    {
        Debug.Log("Exit Idle State");
    }
    public IState OnUpdate(AIEnemyUnit character)
    {

        float distanceToPlayer;
        Vector3 directionToTarget;
        float angle;

        if (character.target != null)
        {
            
            distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);
            directionToTarget = character.transform.position - character.target.position;
            angle = Vector3.Angle(character.transform.forward, directionToTarget);
            //Debug.Log(Mathf.Abs(angle));

            if (distanceToPlayer > character.attackDistance)
            {
                return new PursueState();
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
        else
        {
            if (character.nearestEnemy!=null)
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
                    return new IdleState();
                }
            }
            else
            {
                return new IdleState();
            }
        }

        

        return null;
    }
}
