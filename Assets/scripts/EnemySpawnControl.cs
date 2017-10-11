using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnControl : MonoBehaviour {

    public List<EnemySpawner> AllSpawners= new List<EnemySpawner>();
    public float spawnTimer = 2.5f;

    float textTimer = 2.5f;

    public int TotalDead = 0;

    int CurrentWave = 1;
    int NumInWave = 5;
    int WaveSpawned = 0;
    public int WaveDead = 0;

    private bool IsCoolingDown = true;
    private float CoolDownTimer = 0.0f;

    [SerializeField]
    Text WaveText;

    [SerializeField]
    EnvironmentManager StageManager;

    // Update is called once per frame
    void Update() {

        //When triggered Spawn a zombie and initiate a cooldown
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

        //If the wave has been completed
        if(WaveSpawned == NumInWave && WaveDead == NumInWave)
        {
            if(CurrentWave > 0)
            {
                NumInWave = CurrentWave * 3 + 5;
                GetComponentInChildren<EnemySpawner>().CanSpawnBigEnemies = true;
            }
            CurrentWave++;
            WaveText.gameObject.SetActive(true);
            WaveText.text = "Wave " + (CurrentWave).ToString();
            WaveDead = 0;
            WaveSpawned = 0;
            StageManager.ChangeSet();
        }

        //Manages displaying Text
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
