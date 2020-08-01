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
        fader.GetComponent<Canvas>().sortingOrder = 10;
        fader.FadeOutImmediately();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);
        fader.GetComponent<Canvas>().sortingOrder = 0;
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

    public void DeleteSave()
    {
        GetComponent<SavingSystem>().DeleteSave(defaultSaveFile);
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
