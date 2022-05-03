using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemComplet : MonoBehaviour, IsSaveable
{
    Dictionary<string, object> data = new Dictionary<string, object>();
    private string escena;
    SceneLoader sceneLoader;

    private void Awake()
    {
        if (FindObjectsOfType<SaveSystemComplet>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        sceneLoader = GameObject.Find("/SceneLoader").GetComponent<SceneLoader>();
    }


    public void Save()
    {
        CaptureState();

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "data.save";

        FileStream file = new FileStream(path, FileMode.Create);

        formatter.Serialize(file, data);

        file.Close();
    }

    public void Load()
    {
        SceneManager.LoadScene("ProvesCiria");
        StartCoroutine(Carregar());

    }

    [System.Serializable]
    struct Scene
    {
        public string escena;
    }

    private void CaptureState()
    {
        PersistentGameObject[] persistentGameObjects = FindObjectsOfType<PersistentGameObject>();

        foreach(PersistentGameObject pgo in persistentGameObjects)
        {
            data[pgo.GUID] = pgo.CaptureState();
        }
    }

    private void RestoreState()
    {
        PersistentGameObject[] objectsToLoad = FindObjectsOfType<PersistentGameObject>();

        foreach (PersistentGameObject otl in objectsToLoad)
        {
            otl.RestoreState(data[otl.GUID]);
        }
    }

    IEnumerator Carregar()
    {

        string path = Application.persistentDataPath + "data.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as Dictionary<string, object>;

            stream.Close();

        }
        sceneLoader.gameObject.GetComponent<PersistentGameObject>().RestoreState(data[sceneLoader.gameObject.GetComponent<PersistentGameObject>().GUID]);

        yield return SceneManager.LoadSceneAsync(sceneLoader.escena);
        

        RestoreState();
    }

    object IsSaveable.CaptureState()
    {
        Scene data;
        data.escena = SceneManager.GetActiveScene().name;
        return data;
    }

    void IsSaveable.RestoreState(object data)
    {
        Scene escena = (Scene)data;
        SceneManager.LoadScene(escena.escena);
    }
}
