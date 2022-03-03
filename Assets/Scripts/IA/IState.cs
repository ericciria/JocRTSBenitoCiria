using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnterState(AICharacter character);
    void OnEnterState(PatrollingEnemy character);
    void OnEnterState(AITurret character);
    void OnEnterState(AIEnemyUnit character);

    void OnExitState(AICharacter character);
    void OnExitState(PatrollingEnemy character);
    void OnExitState(AITurret character);
    void OnExitState(AIEnemyUnit character);

    IState OnUpdate(AICharacter character);
    IState OnUpdate(PatrollingEnemy character);
    IState OnUpdate(AITurret character);
    IState OnUpdate(AIEnemyUnit character);
}
