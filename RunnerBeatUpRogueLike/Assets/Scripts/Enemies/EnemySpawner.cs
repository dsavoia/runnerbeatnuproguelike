using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public List<GameObject> enemyList;
    public List<Transform> spawnPoints;
    
    public float minSpawnTime = 2;
    public float maxSpawnTime = 6;
    public int maxSpawnQty = 2;
    
    float timeToNextSpawn = 0;
    bool isSpawning = false;
    public PlayerInfo playerInfo;
    
    public int currentEnemyQty = 0;

	// Use this for initialization
	void Start ()
    {        
        EventManager.onEnemyDeath += EnemyDied;
        EventManager.onEnemyArrivedInTown += EnemyArrivedInTown;
        EventManager.onEnemiesAttakingTown += EnemiesAttackingTown;
    }

    void OnDisable()
    {
        EventManager.onEnemyDeath -= EnemyDied;
        EventManager.onEnemyArrivedInTown -= EnemyArrivedInTown;
        EventManager.onEnemiesAttakingTown -= EnemiesAttackingTown;
    }

    // Update is called once per frame
    void Update ()
    {
        switch (playerInfo.state)
        {
            case PlayerInfo.PlayerState.Dead:
            case PlayerInfo.PlayerState.MovingToTown:
                CancelInvoke("SpawnEnemy");
            break;
            default:                
                    if (currentEnemyQty < maxSpawnQty)
                    {
                        if (timeToNextSpawn == 0 && !isSpawning)
                        {
                            timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
                            Invoke("SpawnEnemy", timeToNextSpawn);
                            //print("time to spawn " + timeToNextSpawn);
                            currentEnemyQty++;
                            isSpawning = true;
                        }
                    }               
            break;
        }
        
	}

    void SpawnEnemy()
    {
        Instantiate(enemyList[Random.Range(0, enemyList.Count)], spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
        isSpawning = false;
        timeToNextSpawn = 0;
    }

    void EnemyDied(BaseEnemy enemy)
    {
        currentEnemyQty--;
    }

    void EnemyArrivedInTown(BaseEnemy enemy)
    {
        //print(gameObject.name + ": Enemy arrived in town");
        currentEnemyQty--;
    }

    void EnemiesAttackingTown()
    {
        CancelInvoke("SpawnEnemy");
    }
}
