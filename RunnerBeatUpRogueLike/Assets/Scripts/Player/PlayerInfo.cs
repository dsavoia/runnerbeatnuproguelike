using UnityEngine;

public class PlayerInfo : MonoBehaviour{

    public enum PlayerState
    {
        Moving,
        Fighting,        
        Dead
    }

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
        state = PlayerState.Moving;
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

    public void ChangeState(PlayerState newState)
    {
        state = newState;
    }

    void UpdateInfo()
    {
        if (facingRight)
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
