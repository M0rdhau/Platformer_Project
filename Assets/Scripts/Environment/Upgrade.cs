using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public enum UpgradeType
    {
        Meditation,
        DoubleJump,
        FireFist,
        Shadow
    }

    [SerializeField] UpgradeType type;
    [SerializeField] string message;
    bool isPickedUp = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPickedUp)
        {
            ProcessUpgrade(collision.gameObject);
        }
    }

    private void ProcessUpgrade(GameObject player)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isPickedUp = true;
            GetComponent<ParticleSystem>().Play();
            Debug.Log("You have picked up " + message + "!");
            player.GetComponent<PlayerUpgrades>().SetUpgrade(type);
        }
    }
}
