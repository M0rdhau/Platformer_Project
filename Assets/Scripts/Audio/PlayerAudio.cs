using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    //0-1 steps
    //2 - punch
    //3 - kick
    //4 - jump
    //5 - land

    [SerializeField] AudioClip[] clips;


    public void PlayWalk()
    {
        int i = Random.Range(0, 2);
        AudioSource.PlayClipAtPoint(clips[i], transform.position, 0.5f);
    }

    public void PlayPunch()
    {
        AudioSource.PlayClipAtPoint(clips[2], transform.position);
    }

    public void PlayKick()
    {
        AudioSource.PlayClipAtPoint(clips[3], transform.position);
    }

    public void PlayJump()
    {
        AudioSource.PlayClipAtPoint(clips[4], transform.position);
    }

    public void PlayLand()
    {
        AudioSource.PlayClipAtPoint(clips[5], transform.position);
    }
}
