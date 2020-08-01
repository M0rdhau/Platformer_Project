using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIWrapper : MonoBehaviour
{
    SceneLoader loader;
    SavingWrapper wrapper;

    GameObject StartCanvas;
    GameObject GameSelectCanvas;
    GameObject CreateNewSaveCanvas;


    private void Awake()
    {
        loader = FindObjectOfType<SceneLoader>();
        StartCanvas = transform.Find("StartCanvas").gameObject;
        GameSelectCanvas = transform.Find("GameSelectCanvas").gameObject;
        CreateNewSaveCanvas = transform.Find("CreateNewSaveCanvas").gameObject;
    }

    private void Start()
    {
        wrapper = FindObjectOfType<SavingWrapper>();
    }

    public void ContinueGame()
    {
        loader.LoadNext();
    }

    public void NewAlert()
    {
        StartCanvas.SetActive(false);
        GameSelectCanvas.SetActive(true);
    }

    public void backToMenu()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCanvas.SetActive(true);
    }

    public void NewGame()
    {
        wrapper.DeleteSave();
        loader.LoadNext();
    }
}
