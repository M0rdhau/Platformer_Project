using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIWrapper : MonoBehaviour
{
    SceneLoader loader;

    Canvas StartCanvas;
    Canvas GameSelectCanvas;
    Canvas CreateNewSaveCanvas;


    private void Awake()
    {
        
        loader = FindObjectOfType<SceneLoader>();
        StartCanvas = transform.Find("StartCanvas").GetComponent<Canvas>();
        GameSelectCanvas = transform.Find("GameSelectCanvas").GetComponent<Canvas>();
        CreateNewSaveCanvas = transform.Find("CreateNewSaveCanvas").GetComponent<Canvas>();
    }
}
