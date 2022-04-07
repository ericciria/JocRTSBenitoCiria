using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PersistentGameObject : MonoBehaviour
{
    public string GUID;
    Dictionary<string, object> data = new Dictionary<string, object>();

    public void Update()
    {
        if (Application.isPlaying) return;
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;
        if (string.IsNullOrEmpty(GUID))
        {
            GUID = Guid.NewGuid().ToString();
        }
    }

    public System.Object CaptureState()
    {
        
        IsSaveable[] saveables = GetComponents<IsSaveable>();

        foreach (IsSaveable saveable in saveables)
        {
            data[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return data;
    }

    public void RestoreState(object data)
    {
        Dictionary<string, object> dataToRestore = (Dictionary<string, object>)data;
        IsSaveable[] saveables = GetComponents<IsSaveable>();

        foreach (IsSaveable saveable in saveables)
        {
            saveable.RestoreState(dataToRestore[saveable.GetType().Name]);
        }
    }
    
}
