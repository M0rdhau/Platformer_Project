using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterExit : MonoBehaviour
{
    Collider2D coll;

    [SerializeField] float playerWaitInterval = 1f;

    bool isPlayerEntered = false;

    IEnumerator checkingCoroutine;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isPlayerEntered)
        {
            isPlayerEntered = true;
            if (checkingCoroutine != null) { StopCoroutine(checkingCoroutine); }
            OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (isPlayerEntered)
        {
            isPlayerEntered = false;
            checkingCoroutine = CheckPlayerLeft();
            StartCoroutine(checkingCoroutine);
        }
    }

    private IEnumerator CheckPlayerLeft()
    {
        yield return new WaitForSeconds(playerWaitInterval);
        if (!coll.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            OnPlayerExit();
        }
    }

    protected virtual void OnPlayerExit()
    {
        throw new NotImplementedException();
    }

    protected virtual void OnPlayerEnter()
    {
        return;
    }
}
