using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{

    Door door;

    bool hasGoneThrough = false;

    private void Update()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            if (isTouchingDoors() && !hasGoneThrough)
            {
                hasGoneThrough = true;
                FindObjectOfType<SceneLoader>().DoorLoadScene(door.GetSceneName(), door.GetDoorIndex());
            }
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            hasGoneThrough = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Door>())
        {
            door = other.gameObject.GetComponent<Door>();
        }
    }

    bool isTouchingDoors()
    {
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Doors"));
    }
}
