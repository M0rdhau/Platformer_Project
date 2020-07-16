using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    EnemyHealth health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetHealth() < 200)
        {
            health.DamageHealth(-500);
        }
    }
}
