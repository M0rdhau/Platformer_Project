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
            if (door != null && !hasGoneThrough)
            {
                hasGoneThrough = true;
                SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
                var doorName = door.GetSceneName();
                var doorIndex = door.GetDoorIndex();
                sceneLoader.DoorLoadScene(doorName, doorIndex);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Door>())
        {
            door = null;
        }
    }

    bool isTouchingDoors()
    {
        Debug.Log(GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Doors")));
        return GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Doors"));
    }
}
