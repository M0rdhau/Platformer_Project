using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    bool isGameRunning = true;
    [SerializeField] GameObject pauseCanvas;

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
