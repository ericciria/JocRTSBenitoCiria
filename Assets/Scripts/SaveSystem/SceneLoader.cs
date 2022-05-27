using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour, IsSaveable
{
    public string escena;
    

    private void Awake()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    [System.Serializable]
    struct Scene
    {
        public string escena;
    }


    object IsSaveable.CaptureState()
    {
        Scene data;
        data.escena = SceneManager.GetActiveScene().name;
        return data;
    }

    void IsSaveable.RestoreState(object data)
    {
        Scene scene = (Scene)data;
        escena = scene.escena;
    }
}
