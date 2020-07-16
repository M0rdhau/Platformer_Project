using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterMask : MonoBehaviour
{
    SpriteMask mask;
    Collider2D coll;

    [SerializeField] float playerWaitInterval = 1f;
    float timeSincePlayerLeft = 0f;

    bool isPlayerEntered = false;

    private void Awake()
    {
        mask = GetComponent<SpriteMask>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayerEntered)
        {
            isPlayerEntered = true;
            mask.enabled = true;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
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
            mask.enabled = false;
        }
    }
}
