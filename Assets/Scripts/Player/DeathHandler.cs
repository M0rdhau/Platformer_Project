using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] GameObject deathCanvas;

    public void Die()
    {
        Time.timeScale = 0;
        deathCanvas.SetActive(true);
    }



}
