using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SaveableEntity : MonoBehaviour
{

    [SerializeField] string uniqueId = "";



    public string GetUniqueIdentifier()
    {
        return "";
    }

    public object CaptureState()
    {
        Debug.Log("Capturing state for " + GetUniqueIdentifier());
        return null;
    }

    public void RestoreState(object state)
    {
        Debug.Log("Restoring state for " + GetUniqueIdentifier());
    }

    private void Update()
    {
        if (Application.IsPlaying(gameObject)) { return; }
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;
        SerializedObject serObj = new SerializedObject(this);
        SerializedProperty serProp = serObj.FindProperty("uniqueId");
        if (string.IsNullOrEmpty(serProp.stringValue))
        {
            serProp.stringValue = System.Guid.NewGuid().ToString();
            serObj.ApplyModifiedProperties();
        }

        this.runInEditMode = true;
        Debug.Log("Editing");
    }
}
