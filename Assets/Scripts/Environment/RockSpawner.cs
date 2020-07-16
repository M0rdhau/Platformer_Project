using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] GameObject RockPrefab;
    [SerializeField] float rockSpawnInterval = 4f;
    Collider2D coll;

    bool isPlayerEntered = false;

    [SerializeField]  float playerWaitInterval = 1f;
    float timeSincePlayerLeft = 0f;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayerEntered)
        {
            isPlayerEntered = true;
            StartCoroutine(SpawnRocks());
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!coll.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Debug.Log("Not touching - " + collision.gameObject.name);
        }
        if (isPlayerEntered)
        {
            timeSincePlayerLeft = Time.time;
            StartCoroutine(CheckPlayerLeft());
        }
    }

    private IEnumerator CheckPlayerLeft()
    {
        yield return new WaitForSeconds(playerWaitInterval);
        if (!coll.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            isPlayerEntered = false;
        }
    }




    private IEnumerator SpawnRocks()
    {
        while (isPlayerEntered)
        {
            Instantiate(RockPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(rockSpawnInterval);
        }
    }

}
