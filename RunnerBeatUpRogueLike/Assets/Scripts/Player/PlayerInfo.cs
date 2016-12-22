using UnityEngine;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour{
    
    public enum PlayerState
    {
        MovingForward,
        MovingToPosition,        
        MovingToTarget,
        MovingToTown,
        Fighting,
        EnemiesAttackingTown,        
        Dead
    }

    public int maxHp;
    [HideInInspector] public int currentHp;
    [HideInInspector] public BaseEnemy focusedEnemy = null;

    public Vector3 targetPos;
    public float attackRange;
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;

    public int goldEarned = 0;
    public int maxEnemiesOnTown;
    public int enemiesInTown = 0;

    public Transform townPos;

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
        CheckGoBackToTown();
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
        if(enemiesInTown >= maxEnemiesOnTown)
        {
            EventManager.OnEnemiesAttakingTown();
            state = PlayerState.EnemiesAttackingTown;
        }
    }

    void CheckGoBackToTown()
    {
        if (state == PlayerState.Dead)
        {
            Invoke("GoBackToTown", 3);
            return;
        }

        if (state == PlayerState.EnemiesAttackingTown)
        {
            Invoke("GoBackToTown", 1.5f);
            return;
        }

    }

    void GoBackToTown()
    {
        targetPos = townPos.position;
        state = PlayerState.MovingToTown;
    }
    
    public void LoadTown()
    {
        ///TODO: Load town
    }

}
