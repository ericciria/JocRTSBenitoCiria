using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : IState
{

    /*
     *  Test Enemy
     */
    public void OnEnterState(AICharacter character)
    {
        Debug.Log("Enter Pursue State");
    }
    public void OnExitState(AICharacter character)
    {
        Debug.Log("Exit Pursue State");
    }
    public IState OnUpdate(AICharacter character)
    {
        float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

        if (distanceToPlayer < character.atackDistance)
        {
            character.agent.SetDestination(character.transform.position);
            return new AttackState();
        }
        else if (distanceToPlayer>character.pursueDistance)
        {
            character.agent.SetDestination(character.transform.position);
            return new IdleState();
        }
        Debug.Log("Pursuing");
        character.agent.SetDestination(character.target.position);
        return null;
    }

    /*
     *  Patrolling Enemy
     */
    void IState.OnEnterState(PatrollingEnemy character)
    {
        Debug.Log("Enter Pursue State");
    }
    void IState.OnExitState(PatrollingEnemy character)
    {
        Debug.Log("Exit Pursue State");
    }
    IState IState.OnUpdate(PatrollingEnemy character)
    {
        float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

        if (distanceToPlayer < character.atackDistance)
        {
            character.agent.SetDestination(character.transform.position);
            return new AttackState();
        }
        else if (distanceToPlayer > character.pursueDistance)
        {
            character.agent.SetDestination(character.currentPoint.position);
            return new IdleState();
        }
        Debug.Log("Pursuing");
        character.agent.SetDestination(character.target.position);
        return null;
    }

    /*
     * Turret, no fa res perque no utilitza pursuestate
     */
    public void OnEnterState(AITurret character)
    {
    }
    public void OnExitState(AITurret character)
    {
    }
    public IState OnUpdate(AITurret character)
    {
        return null;
    }

    /*
     *  Patrolling Enemy
     */
    void IState.OnEnterState(AIEnemyUnit character)
    {
        Debug.Log("Enter Pursue State");
    }
    void IState.OnExitState(AIEnemyUnit character)
    {
        Debug.Log("Exit Pursue State");
    }
    IState IState.OnUpdate(AIEnemyUnit character)
    {
        if (character.target != null)
        {
            float distanceToPlayer = Vector3.Distance(character.target.position, character.transform.position);

            if (distanceToPlayer < character.attackDistance)
            {
                character.agent.SetDestination(character.transform.position);
                return new AttackState();
            }
            //Debug.Log("Pursuing");
            character.agent.SetDestination(character.target.position);
        }
        else
        {
            return new IdleState();
        }

        return null;
    }
}
