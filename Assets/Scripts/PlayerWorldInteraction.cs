using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{

    Door door;

    private void Update()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (isTouchingDoors())
            {
                FindObjectOfType<SceneLoader>().DoorLoadScene(door.GetSceneName());
            }   
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
