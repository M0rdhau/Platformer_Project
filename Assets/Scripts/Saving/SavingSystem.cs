using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour
{

    public IEnumerator LoadLastScene(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        if (state.ContainsKey("lastLoadedScene"))
        {
            int sceneNum = (int)state["lastLoadedScene"];
            if (sceneNum != SceneManager.GetActiveScene().buildIndex)
            {
                yield return SceneManager.LoadSceneAsync(sceneNum);
            }
        }
        
        Restore(state);
    }

    public void Save(string saveFile)
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        CaptureState(state);
        SaveFile(saveFile, state);
    }

    public void Load(string saveFile)
    {
        Restore(LoadFile(saveFile));
    }

    private void SaveFile(string saveFile, Dictionary<string, object> capturedState)
    {
        var path = GetPathFromSaveFile(saveFile);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, capturedState);
        }
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        if (!File.Exists(path)) { return new Dictionary<string, object>(); }
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
           BinaryFormatter formatter = new BinaryFormatter();
           return (Dictionary<string, object>) formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
        }
        state["lastLoadedScene"] = SceneManager.GetActiveScene().buildIndex;
    }

    private void Restore(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.ContainsKey(saveable.GetUniqueIdentifier()))
            {
                saveable.RestoreState(state[saveable.GetUniqueIdentifier()]);
            }
        }
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}
