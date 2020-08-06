using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    bool isGameRunning = true;
    SceneLoader loader;
    [SerializeField] GameObject pauseCanvas;

    private void Start()
    {
        loader = FindObjectOfType<SceneLoader>();
    }

    public void ToMainMenuWithoutSave()
    {
        loader.LoadMainMenu(false);
    }

    public void ToMainMenuWithSave()
    {
        loader.LoadMainMenu(true);
    }
    
    //if anyone ever reads this, I know, I know
    //you shouldn't put same dependency in multiple scripts
    //I am sorry, okay?
    public void WrapLoad()
    {
        loader.LoadLastWrap();
    }
    

    public void PauseGame()
    {
        if (isGameRunning)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            isGameRunning = false;
        }
        else
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            isGameRunning = true;
        }
    }
}
