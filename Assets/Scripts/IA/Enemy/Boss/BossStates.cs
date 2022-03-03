using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossStates
{
    void OnEnterState(AIBoss character);

    void OnExitState(AIBoss character);

    BossStates OnUpdate(AIBoss character);
}
