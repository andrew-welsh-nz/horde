using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    GameObject TestSpawnPrefab;

    private GameObject enemyPrefab;
    private GameObject BigEnemyPrefab;
    private SpecialEnemy SpecialEnemy1;
    private SpecialEnemy SpecialEnemy2;
    private SpecialEnemy SpecialEnemy3;

    [SerializeField]
    float MaxSpawnDistanceVariance;

    bool AttemptingSpawn = false;
    EnemySpawnControl SpawnControl;

    public int WaveCounter = 1;

    void Awake()
    {
        SpawnControl = GetComponentInParent<EnemySpawnControl>();
        SpawnControl.AllSpawners.Add(this);
    }

    private void Start(){
        enemyPrefab = SpawnControl.enemyPrefab;
        BigEnemyPrefab = SpawnControl.BigEnemyPrefab;
        SpecialEnemy1 = SpawnControl.SpecialEnemy1;
        SpecialEnemy2 = SpawnControl.SpecialEnemy2;
        SpecialEnemy3 = SpawnControl.SpecialEnemy3;
    }

    IEnumerator SpawningEnemy()
    {
        AttemptingSpawn = true;
            do
            {
                //Creates a small variance to the spawning so its a spawn cricle not a spawn point
                Vector3 SpawnVariance = new Vector3(Random.Range(-MaxSpawnDistanceVariance, MaxSpawnDistanceVariance), 0.0f, Random.Range(-MaxSpawnDistanceVariance, MaxSpawnDistanceVariance));

                GameObject Tester = Instantiate(TestSpawnPrefab, transform.position + SpawnVariance, Quaternion.identity);
                yield return new WaitForFixedUpdate();

            if (Tester.GetComponent<SpawnTester>().allClear)
                {
                    Destroy(Tester);


                float RemainingPercent = 100.0f;
                //Spawn the Enemy

                //Check Spawning chance for Special Enemy 1
                if (SpecialEnemy1.Prefab != null && SpecialEnemy1.StartSpawningWave <= WaveCounter)
                {
                    if (Random.Range(0.0f, 100.0f) <= SpecialEnemy1.SpawnChance *(RemainingPercent / 100.0f))
                    {
                        GameObject zombie = Instantiate(SpecialEnemy1.Prefab, transform.position + SpawnVariance, Quaternion.identity);
                        zombie.GetComponent<ZombieController>().controller = SpawnControl;
                        AttemptingSpawn = false;
                        break;
                    }
                    RemainingPercent -= SpecialEnemy1.SpawnChance;
                }

                //Check Spawning chance for Special Enemy 2
                if (SpecialEnemy2.Prefab != null && SpecialEnemy2.StartSpawningWave <= WaveCounter)
                {
                    if (Random.Range(0.0f, 100.0f) <= (SpecialEnemy2.SpawnChance * (RemainingPercent / 100.0f)))
                    {
                        GameObject zombie = Instantiate(SpecialEnemy2.Prefab, transform.position + SpawnVariance, Quaternion.identity);
                        zombie.GetComponent<ZombieController>().controller = SpawnControl;
                        AttemptingSpawn = false;
                        break;
                    }
                    RemainingPercent -= SpecialEnemy2.SpawnChance;
                }

                //Check Spawning chance for Special Enemy 3
                if (SpecialEnemy3.Prefab != null && SpecialEnemy3.StartSpawningWave <= WaveCounter)
                {
                    if (Random.Range(0.0f, 100.0f) <= (SpecialEnemy3.SpawnChance *(RemainingPercent / 100.0f)))
                    {
                        GameObject zombie = Instantiate(SpecialEnemy3.Prefab, transform.position + SpawnVariance, Quaternion.identity);
                        zombie.GetComponent<ZombieController>().controller = SpawnControl;
                        AttemptingSpawn = false;
                        break;
                    }
                    RemainingPercent -= SpecialEnemy3.SpawnChance;
                }

                //Check Spawning chance for Big Enemy otherwise spawn Regular enemy
                if (Random.Range(0.0f, 100.0f) <= 30.0f){
                        GameObject zombie = Instantiate(BigEnemyPrefab, transform.position + SpawnVariance, Quaternion.identity);
                        zombie.GetComponent<ZombieController>().controller = SpawnControl;
                    }
                    else {
                        GameObject zombie = Instantiate(enemyPrefab, transform.position + SpawnVariance, Quaternion.identity);
                        zombie.GetComponent<ZombieController>().controller = SpawnControl;
                    }

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
