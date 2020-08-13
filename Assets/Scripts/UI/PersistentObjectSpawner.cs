using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{

    [SerializeField] GameObject persistentObjPrefab;

    static bool hasSpawned = false;


    private void Awake()
    {
        if (hasSpawned) { return; }

        SpawnPersistentObjects();

        hasSpawned = true;
    }

    private void Start()
    {
        FindObjectOfType<AudioPlayer>().CheckMusic();
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObjects = Instantiate(persistentObjPrefab);

        DontDestroyOnLoad(persistentObjects);
    }

 
}
