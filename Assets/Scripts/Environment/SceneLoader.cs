using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float fadeInTime = 1f;
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
        StartCoroutine(DoorLoadCoroutine(sceneName, doorIndex));
    }


    private IEnumerator DoorLoadCoroutine(string sceneName, int doorIndex)
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        DontDestroyOnLoad(gameObject);
        wrapper.Save();
        //changin timescale so that everything stays the same on the screen
        yield return FindObjectOfType<Fader>().FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneName);
        Door doorToSpawnAt = FindDoor(doorIndex);
        wrapper.Load();
        SetPlayerLocation(doorToSpawnAt);
        wrapper.Save();
        yield return new WaitForSeconds(waitTime);
        yield return FindObjectOfType<Fader>().FadeIn(fadeInTime);
        Destroy(gameObject);
    }

    private void SetPlayerLocation(Door doorToSpawnAt)
    {
        var spawnPoint = doorToSpawnAt.GetSpawnPoint();
        var player = FindObjectOfType<PlayerController>().gameObject;
        player.transform.position = spawnPoint.position;
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }

    #endregion
}
