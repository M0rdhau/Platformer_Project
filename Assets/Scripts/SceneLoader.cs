using System;
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

    public void LoadNext()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DoorLoadScene(string sceneName, int doorIndex)
    {
        StartCoroutine(Transition(sceneName, doorIndex));
    }

    private IEnumerator Transition(string sceneName, int doorIndex)
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneName);
        Door doorToSpawnAt = FindDoor(doorIndex);
        SetPlayerLocation(doorToSpawnAt);
        Destroy(gameObject);
    }

    private void SetPlayerLocation(Door doorToSpawnAt)
    {
        var spawnPoint = doorToSpawnAt.GetSpawnPoint();
        FindObjectOfType<PlayerController>().gameObject.transform.position = spawnPoint.position;
    }

    private Door FindDoor(int doorIndex)
    {
        var doors = FindObjectsOfType<Door>();
        //inline extravaganza
        foreach (Door door in doors)
        {
            if (door.GetDoorIndex() == doorIndex)
            {
                return door;
            }
        }
        return null;
    }

    #region Old Functions - not needed right now

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

    #endregion
}
