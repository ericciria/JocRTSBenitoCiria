using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void OnEnterState(AICharacter character)
    {
        Debug.Log("Enter Idle State");
    }
    public void OnExitState(AICharacter character)
    {
        Debug.Log("Exit Idle State");
    }
    public IState OnUpdate(AICharacter character)
    {
        float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

        if (distanceToPlayer < character.pursueDistance)
        {
            return new PursueState();
        }
        Debug.Log("Idling");
        return null;
    }

    /*
     * Patrolling Enemy, Utilitzo l'Idle com a defecte per patrullar
     */
    void IState.OnEnterState(PatrollingEnemy character)
    {
        Debug.Log("Enter Patrol State");
    }
    void IState.OnExitState(PatrollingEnemy character)
    {
        Debug.Log("Exit Patrol State");
    }
    IState IState.OnUpdate(PatrollingEnemy character)
    {
        if (character.nearestEnemy != null)
        {
            float distanceToPlayer = Vector3.Distance(character.nearestEnemy.transform.position, character.transform.position);
            if (distanceToPlayer < character.pursueDistance)
            {
                return new PursueState();
            }
        }
        
        //Debug.Log("Patrolling");
        character.agent.SetDestination(character.currentPoint.position);
        return null;
    }


    /*
     * Turret
     */
    public void OnEnterState(AITurret character)
    {
        Debug.Log("Enter Idle State");
        character.target = character.originalPosition;
    }
    public void OnExitState(AITurret character)
    {
        Debug.Log("Exit Idle State");
    }
    public IState OnUpdate(AITurret character)
    {
        if (character.nearestEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(character.nearestEnemy.transform.position, character.transform.position);
            if (distanceToTarget < character.attackDistance)
            {
                return new AttackState();
            }
        }
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(character.target.position - character.transform.position), 1 * Time.deltaTime);
        return null;
    }

    /*
     * Turret
     */
    public void OnEnterState(AIEnemyUnit character)
    {
        Debug.Log("Enter Idle State");
        character.target = null;
    }
    public void OnExitState(AIEnemyUnit character)
    {
        Debug.Log("Exit Idle State");
    }
    public IState OnUpdate(AIEnemyUnit character)
    {
        if (character.target != null)
        {
            return new PursueState();
        }
        else if (character.nearestEnemy != null)
        {
            float distanceToTarget = Vector3.Distance(character.nearestEnemy.transform.position, character.transform.position);
            if (distanceToTarget < character.attackDistance)
            {
                return new AttackState();
            }
        }
        return null;
    }
}
