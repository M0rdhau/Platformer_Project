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
    int mainMenuIndex = 0;
    Fader fader;
    SavingWrapper wrapper;


    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        fader = FindObjectOfType<Fader>();
        wrapper = FindObjectOfType<SavingWrapper>();
        //if (currentScene.buildIndex == 0)
        //{
        //    StartCoroutine(MainMenuCoroutine(currentScene.buildIndex));
        //}
    }

    public void LoadNext()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadWithFader(currentScene + 1));
        //SceneManager.LoadScene(currentScene + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DoorLoadScene(string sceneName, int doorIndex)
    {
        StartCoroutine(DoorLoadCoroutine(sceneName, doorIndex));
    }

    private IEnumerator LoadWithFader(int sceneNumber)
    {
        //DontDestroyOnLoad(gameObject);
        wrapper.Save();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneNumber);
        wrapper.Load();
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        //Destroy(gameObject);
    }


    private IEnumerator DoorLoadCoroutine(string sceneName, int doorIndex)
    {
        //DontDestroyOnLoad(gameObject);
        wrapper.Save();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneName);
        Door doorToSpawnAt = FindDoor(doorIndex);
        wrapper.Load();
        if (doorToSpawnAt != null) { SetPlayerLocation(doorToSpawnAt); }
        //wrapper.Save();
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        //Destroy(gameObject);
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

    public void LoadLastWrap()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        //DontDestroyOnLoad(gameObject);
        wrapper.Save();
        yield return fader.FadeOut(fadeOutTime);
        wrapper.LoadLastScene();
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        //Destroy(gameObject);
    }

    public void LoadMainMenu(bool shouldSave)
    {
        StartCoroutine(MainMenuCoroutine(shouldSave));
    }

    private IEnumerator MainMenuCoroutine(bool shouldReload)
    {
        //DontDestroyOnLoad(gameObject);
        if (shouldReload) { wrapper.Save(); }
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(mainMenuIndex);
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        //Destroy(gameObject);
    }

    public void TryAgain()
    {
        StartCoroutine(TryAgainCoroutine());
    }

    private IEnumerator TryAgainCoroutine()
    {
        //DontDestroyOnLoad(gameObject);
        yield return fader.FadeOut(fadeOutTime);
        wrapper.Load();
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        //Destroy(gameObject);
    }

    #region Old Functions - not needed right now



    public void Lose()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    public void ReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

    #endregion
}
