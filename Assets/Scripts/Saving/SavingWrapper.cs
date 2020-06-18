using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save";

    float fadeInTime = 0.5f;

    private IEnumerator Start()
    {
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediately();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Load();
        }

    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }
}
