using UnityEngine;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour{
    
    public enum PlayerState
    {
        MovingForward,
        MovingToPosition,        
        MovingToTarget,
        Fighting,        
        Dead
    }

    public int maxHp;
    [HideInInspector]public int currentHp;
    
    public Vector3 targetPos;
    //public bool goingToTargetpos;
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;

    public int goldEarned = 0;
    public int maxEnemiesOnTown;
    public int enemiesInTown = 0;

    public PlayerState state;   

    void Start()
    {
        EventManager.onEnemyDeath += EnemyDied;
        EventManager.onEnemyArrivedInTown += EnemyArrivedInTown;
        currentHp = maxHp;
        state = PlayerState.MovingForward;
    }

    void OnDisable()
    {
        EventManager.onEnemyDeath -= EnemyDied;
        EventManager.onEnemyArrivedInTown -= EnemyArrivedInTown;
    }


    void Update()
    {
        UpdateInfo();
    }

    public void SetState(PlayerState newState)
    {
        state = newState;
    }

    void UpdateInfo()
    {
        if (state == PlayerState.MovingForward)
        {
            distanceWalked += Time.deltaTime;
        }
    }

    void EarnGold(int amount)
    {
        goldEarned += amount;
    }

    void EnemyDied(BaseEnemy enemy)
    {
        EarnGold(enemy.goldValue);
    }

    void EnemyArrivedInTown(BaseEnemy enemy)
    {
        //print(gameObject.name + ": Enemy arrived in town");
        enemiesInTown++;
    }

}
