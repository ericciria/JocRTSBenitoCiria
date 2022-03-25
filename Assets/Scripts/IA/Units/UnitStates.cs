using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UnitStates
{
    void OnEnterState(Unit unit);
    void OnExitState(Unit unit);
    UnitStates OnUpdate(Unit unit);
}
