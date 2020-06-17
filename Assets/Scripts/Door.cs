using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] string sceneName;

    public string GetSceneName()
    {
        return sceneName;
    }
    
}
