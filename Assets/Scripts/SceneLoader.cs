using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float secondsToLoad = 3f;
    int mainMenuIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //if (currentScene.buildIndex == 0)
        //{
        //    StartCoroutine(MainMenuCoroutine(currentScene.buildIndex));
        //}
    }

    public void DoorLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Lose()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    public void ReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator MainMenuCoroutine(int sceneInd)
    {
        yield return new WaitForSeconds(secondsToLoad);
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void LoadNext()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
