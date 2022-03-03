using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerUnitStates
{
    void OnEnterState(AIPlayerunit character);
    void OnExitState(AIPlayerunit character);
    PlayerUnitStates OnUpdate(AIPlayerunit character);
}
