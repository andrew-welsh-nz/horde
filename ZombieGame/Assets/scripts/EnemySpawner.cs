using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject TestSpawnPrefab;
    [SerializeField]
    GameObject BigEnemyPrefab;

    [SerializeField]
    float MaxSpawnDistanceVariance;

    bool AttemptingSpawn = false;
    EnemySpawnControl SpawnControl;

    public bool CanSpawnBigEnemies = false;

    void Awake()
    {
        SpawnControl = GetComponentInParent<EnemySpawnControl>();
        SpawnControl.AllSpawners.Add(this);
    }

    IEnumerator SpawningEnemy()
    {
        AttemptingSpawn = true;
        Debug.Log("Spawn Enemy Function");
            do
            {
                //Creates a small variance to the spawning so its a spawn cricle not a spawn point
                Vector3 SpawnVariance = new Vector3(Random.Range(-MaxSpawnDistanceVariance, MaxSpawnDistanceVariance), 0.0f, Random.Range(-MaxSpawnDistanceVariance, MaxSpawnDistanceVariance));

                GameObject Tester = Instantiate(TestSpawnPrefab, transform.position + SpawnVariance, Quaternion.identity);
                yield return new WaitForFixedUpdate();

            if (Tester.GetComponent<SpawnTester>().allClear)
                {
                    Destroy(Tester);

                if (CanSpawnBigEnemies){
                    //40% chance, spawn Big enemy
                    if (Random.Range(0.0f, 1.0f) <= 0.4f){
                        Instantiate(BigEnemyPrefab, transform.position + SpawnVariance, Quaternion.identity);
                    }
                    else {
                        Instantiate(enemyPrefab, transform.position + SpawnVariance, Quaternion.identity);
                    }
                }
                else
                {
                    Instantiate(enemyPrefab, transform.position + SpawnVariance, Quaternion.identity);
                }

                Debug.Log("Spawning Prefab");

                AttemptingSpawn = false;
                break;

                }
                else
                {
                    Debug.Log("Collision Occured");
                }
            } while (true);
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawningEnemy");
    }
}
