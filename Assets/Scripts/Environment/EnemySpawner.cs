using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnerDimensions = new Vector3(10f, 0.5f, 0f);
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float secondsBetweenSpawns = 2f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private Vector2 movementVec = new Vector2 (2f, 0f);

    bool isSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnMonsters());
    }

    private IEnumerator spawnMonsters()
    {
        isSpawning = true;
        while (transform.childCount <= maxEnemies)
        {
            Vector3 location = transform.position;
            location.x = Random.Range(-0.5f * spawnerDimensions.x + location.x, 0.5f * spawnerDimensions.x + location.x);
            var enem = Instantiate(enemyPrefab, location, transform.rotation, this.transform);
            enem.GetComponent<EnemyMovement>().SetMovementVector(movementVec);
            yield return new WaitForSeconds(secondsBetweenSpawns);
        }
        isSpawning = false;
    }


    private void Update()
    {
        if(!isSpawning && transform.childCount <= maxEnemies) StartCoroutine(spawnMonsters());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnerDimensions);
    }
}
