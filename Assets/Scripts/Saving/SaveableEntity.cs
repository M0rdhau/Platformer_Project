using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SaveableEntity : MonoBehaviour
{
    [SerializeField] string uniqueId = "";

    static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();


    public string GetUniqueIdentifier()
    {
        return uniqueId;
    }

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }
        return state;
        //return new SerializableVector(transform.position);
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            string typeString = saveable.GetType().ToString();
            if (stateDict.ContainsKey(typeString))
            {
                saveable.RestoreState(stateDict[typeString]);
            }
        }

        //SerializableVector vec = (SerializableVector)state;
        //transform.position = vec.GetVector();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.IsPlaying(gameObject)) { return; }
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;
        SerializedObject serObj = new SerializedObject(this);
        SerializedProperty serProp = serObj.FindProperty("uniqueId");
        if (string.IsNullOrEmpty(serProp.stringValue) || !IsUnique(serProp.stringValue))
        {
            serProp.stringValue = System.Guid.NewGuid().ToString();
            serObj.ApplyModifiedProperties();
        }
        globalLookup[serProp.stringValue] = this;
    }

    private bool IsUnique(string stringValue)
    {
        if (!globalLookup.ContainsKey(stringValue)) return true;

        if (globalLookup[stringValue] == this) return true;

        if (globalLookup[stringValue] == null)
        {
            globalLookup.Remove(stringValue);
            return true;
        }

        if (globalLookup[stringValue].GetUniqueIdentifier() != stringValue)
        {
            globalLookup.Remove(stringValue);
            return true;
        }

        return false;
        
    }

#endif
}
