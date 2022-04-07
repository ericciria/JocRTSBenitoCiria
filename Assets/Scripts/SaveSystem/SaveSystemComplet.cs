using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystemComplet : MonoBehaviour
{
    Dictionary<string, object> data = new Dictionary<string, object>();

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
        

        string path = Application.persistentDataPath + "data.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as Dictionary<string, object>;

            stream.Close();

        }

        RestoreState();

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
}
