using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        print("Saving to " + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, CaptureState());
        }
    }



    public void Load(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        print("Loading from " + path);
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Restore(formatter.Deserialize(stream));
        }

    }

    private object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
        }
        return state;
    }

    private void Restore(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
        }
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}
