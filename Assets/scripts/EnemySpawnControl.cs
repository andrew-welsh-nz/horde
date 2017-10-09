using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnControl : MonoBehaviour {

    public List<EnemySpawner> AllSpawners= new List<EnemySpawner>();
    public float spawnTimer = 3.0f;

    float textTimer = 2.5f;

    public int TotalDead = 0;

    int CurrentWave = 0;
    int NumInWave = 5;
    int WaveSpawned = 0;
    public int WaveDead = 0;

    private bool IsCoolingDown = true;
    private float CoolDownTimer = 0.0f;
    private float TimeSinceStart = 0.0f;

    [SerializeField]
    Text WaveText;

    // Update is called once per frame
    void Update() {

        TimeSinceStart += Time.deltaTime;
        if (TimeSinceStart >= 30.0f){
            foreach (EnemySpawner Spawner in AllSpawners) {
                Spawner.CanSpawnBigEnemies = true;
            }
        }

        if (!IsCoolingDown && WaveSpawned < NumInWave) {
            StartCoroutine(SpawningEnemy());
            IsCoolingDown = true;
            WaveSpawned++;
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

        if(WaveSpawned == NumInWave && WaveDead == NumInWave)
        {
            if(CurrentWave > 0)
            {
                NumInWave *= 2;
            }
            CurrentWave++;
            WaveText.gameObject.SetActive(true);
            WaveText.text = "Wave " + CurrentWave.ToString();
            WaveDead = 0;
            WaveSpawned = 0;
        }

        if (WaveText.IsActive())
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0.0f)
            {
                textTimer = 2.5f;
                WaveText.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator SpawningEnemy()  {
        AllSpawners[Random.Range(0, AllSpawners.Count)].StartSpawning();
        yield return new WaitForSeconds(0);
    }
}
