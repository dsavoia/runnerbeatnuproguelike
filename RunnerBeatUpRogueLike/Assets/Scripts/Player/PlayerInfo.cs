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

    
    [HideInInspector] public int currentHp;
    [HideInInspector] public BaseEnemy focusedEnemy = null;
    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public bool isBasicAttackOnCooldown = false;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public List<BaseEnemy> engagedEnemies;

    public int maxHp;
    public float basicAttackRange;
    public float basicAttackCooldown;
    public int basicAttackDamage;
    public float speed;    
    public bool facingRight = true;
    public float distanceWalked = 0;

    public int goldEarned = 0;
    public int maxEnemiesInTown;
    public int enemiesInTown = 0;

    public LayerMask interactiveObjectsLayer;

    public Transform townPos;

    public PlayerState state; 

    void Start()
    {        
        EventManager.onEnemyDeath += EnemyDied;
        EventManager.onEnemyArrivedInTown += EnemyArrivedInTown;
        currentHp = maxHp;
        engagedEnemies = new List<BaseEnemy>();
        state = PlayerState.MovingForward;
    }

    void OnDisable()
    {
        EventManager.onEnemyDeath -= EnemyDied;
        EventManager.onEnemyArrivedInTown -= EnemyArrivedInTown;
    }


    void Update()
    {
        //CheckEngagedEnemies();
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
        if(engagedEnemies.Contains(enemy))
        {
            engagedEnemies.Remove(enemy);
        }

        focusedEnemy = null;
        EarnGold(enemy.goldValue);        
        
        if (engagedEnemies.Count > 0)
        {
            focusedEnemy = engagedEnemies[0];
            focusedEnemy.SetFocus(true);
            targetObject = focusedEnemy.gameObject;
            state = PlayerState.MovingToTarget;
            return;
        }              
        
        state = PlayerState.MovingForward;
    }

    void EnemyArrivedInTown(BaseEnemy enemy)
    {
        //print(gameObject.name + ": Enemy arrived in town");
        enemiesInTown++;

        if (engagedEnemies.Contains(enemy))
        {
            engagedEnemies.Remove(enemy);
        }

        if (enemiesInTown >= maxEnemiesInTown)
        {
            EventManager.OnEnemiesAttakingTown();
            focusedEnemy = null;
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
}
