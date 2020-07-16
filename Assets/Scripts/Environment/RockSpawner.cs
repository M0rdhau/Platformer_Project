using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : PlayerEnterExit
{
    [SerializeField] GameObject RockPrefab;
    [SerializeField] float rockSpawnInterval = 4f;

    IEnumerator rockSpawn;

    private void Start()
    {
        rockSpawn = null;
    }


    protected override void OnPlayerEnter()
    {
        if (rockSpawn == null)
        {
            rockSpawn = SpawnRocks();
            StartCoroutine(rockSpawn);
        }
    }


    protected override void OnPlayerExit()
    {
        StopCoroutine(rockSpawn);
        rockSpawn = null;
    }

    private IEnumerator SpawnRocks()
    {
        while (true)
        {
            Instantiate(RockPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(rockSpawnInterval);
        }
    }

}
