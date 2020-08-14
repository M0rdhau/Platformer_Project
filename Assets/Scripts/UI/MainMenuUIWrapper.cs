using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIWrapper : MonoBehaviour
{
    SavingWrapper wrapper;

    GameObject StartCanvas;
    GameObject GameSelectCanvas;
    GameObject CreateNewSaveCanvas;

    //it would have been better if this could have been in the awake method, but 
    //that will need me to make a loader wrapper which I won't do
    //because it's largely the same

    private void Start()
    {
        wrapper = FindObjectOfType<SavingWrapper>();
        
        StartCanvas = transform.Find("StartCanvas").gameObject;
        GameSelectCanvas = transform.Find("GameSelectCanvas").gameObject;
        CreateNewSaveCanvas = transform.Find("CreateNewSaveCanvas").gameObject;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        loader.LoadLastWrap();
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
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        wrapper.DeleteSave();
        loader.LoadNext();
    }
}
