using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnBehavior : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnRate = 5.0f;
    float spawnTimer = 0.0f;
    [SerializeField] Transform[] spawnPointTransforms;
    private GameObject[] spawnedEnemies;
    [Range(1, 12)]
    [SerializeField] int spawnCount = 3;

    // Update is called once per frame
    void Update()
    {
        //if()
        //{

        //}
        ////as long as the spawner has NOT already spawned enough bugs,...
        //if (gameObject.transform.childCount - 2 < spawnCount)
        //{
        //    if (spawnTimer >= spawnRate)
        //    {
        //        //spawn a bug at spawnPointTransform and set its parent to this spawner
        //        Instantiate(enemyPrefab, spawnPointTransform.position, Quaternion.identity, gameObject.transform);

        //        //reset spawnTimer
        //        spawnTimer = 0.0f;
        //    }
        //    else
        //    {
        //        //increment spawnTimer
        //        spawnTimer += Time.deltaTime;
        //    }
        //}
    }

    public void SpawnNewWave()
    {
        //make sure all enemies in spawnedEnemies are destroyed
        for(int i = 0; i < spawnedEnemies.Length; i++)
        {
            Destroy(spawnedEnemies[i]);
        }

        //determine the number of enemies in this wave
        int numberOfEnemiesToSpawn = (int)Random.Range(0f, (float)spawnCount);

        //
        int[] usedSpawnpoints = new int[spawnPointTransforms.Length];

        //spawn numberOfEnemiesToSpawn enemies at random but different spots
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            //if(usedSpawnpoints)
            Instantiate(enemyPrefab, spawnPointTransforms[i].position, Quaternion.identity);
        }
    }
}
