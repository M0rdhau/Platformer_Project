using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    enum DestinationDoor
    {
        A, B, C, D, E, F
    }

    [SerializeField] string sceneName;
    [SerializeField] int portalIndex = 0;


    public string GetSceneName() { return sceneName; }

    public Transform GetSpawnPoint() { return transform.GetChild(0); }

    public int GetDoorIndex(){ return portalIndex; }
    
}
