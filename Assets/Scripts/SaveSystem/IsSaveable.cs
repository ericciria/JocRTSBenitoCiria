using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsSaveable
{
    object CaptureState();

    void RestoreState(object data);
}
