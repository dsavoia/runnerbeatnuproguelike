using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour 
{
    public delegate void EnemyDied(BaseEnemy enemy);
    public static event EnemyDied onEnemyDeath;

    public delegate void EnemyArrivedInTown(BaseEnemy enemy);
    public static event EnemyArrivedInTown onEnemyArrivedInTown;

    public delegate void EnemiesAttakingTown();
    public static event EnemiesAttakingTown onEnemiesAttackingTown;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDeath;

    public static void OnEnemyDeath(BaseEnemy enemy)
    {
        if(onEnemyDeath != null)
        {
            onEnemyDeath(enemy);
        }
    }

    public static void OnEnemyArrivedInTown(BaseEnemy enemy)
    {
        if (onEnemyArrivedInTown != null)
        {
            onEnemyArrivedInTown(enemy);
        }
    }

    public static void OnEnemiesAttackingTown()
    {
        if (onEnemiesAttackingTown != null)
        {
            onEnemiesAttackingTown();
        }
    }

    public static void OnPlayerDeath()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }    
}