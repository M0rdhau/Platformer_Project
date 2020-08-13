using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    SceneLoader loader;
    AudioSource _source;

    bool isInTown = false;

    public void CheckMusic()
    {
        _source = GetComponent<AudioSource>();
        loader = FindObjectOfType<SceneLoader>();
        if (!isInTown && (loader.GetSceneName() == "Main Menu" || loader.GetSceneName() == "Tutorial"))
        {
            isInTown = true;
            _source.clip = clips[0];
            _source.Play();
        }

        if (loader.GetSceneName() == "SecondLevel")
        {
            isInTown = false;
            _source.clip = clips[1];
            _source.Play();
        }
        if (loader.GetSceneName() == "BossLevel")
        {
            isInTown = false;
            _source.clip = clips[2];
            _source.Play();
        }
    }
}
