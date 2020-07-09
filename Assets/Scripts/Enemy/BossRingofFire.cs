using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRingofFire : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(0, 0, 10);
    [SerializeField] float damage = 2f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().DamageHealth(damage);
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation*Time.deltaTime);
    }
}
