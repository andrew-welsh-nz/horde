using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnControl : MonoBehaviour {

    public List<EnemySpawner> AllSpawners= new List<EnemySpawner>();
    public float spawnTimer = 3.0f;

    public int TotalDead = 0;

    private bool IsCoolingDown = true;
    private float CoolDownTimer = 0.0f;
    private float TimeSinceStart = 0.0f;

    // Update is called once per frame
    void Update() {

        TimeSinceStart += Time.deltaTime;
        if (TimeSinceStart >= 30.0f){
            foreach (EnemySpawner Spawner in AllSpawners) {
                Spawner.CanSpawnBigEnemies = true;
            }

        }

        if (!IsCoolingDown) {
            StartCoroutine(SpawningEnemy());
            IsCoolingDown = true;
        }
        else {
            CoolDownTimer += Time.deltaTime;
        }

        if (CoolDownTimer >= spawnTimer) {
            IsCoolingDown = false;
            CoolDownTimer = 0.0f;
        }

        if (spawnTimer > 0.3f) {
            spawnTimer -= Time.deltaTime * (2.0f / 90.0f);
        }
        else {
            spawnTimer = 0.3f;
        }

	}

    IEnumerator SpawningEnemy()  {
        AllSpawners[Random.Range(0, AllSpawners.Count)].StartSpawning();
        yield return new WaitForSeconds(0);
    }


}
