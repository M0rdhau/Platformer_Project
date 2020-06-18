using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

    }
}
