using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour 
{
    public delegate void EnemyDied(BaseEnemy enemy);
    public static event EnemyDied onEnemyDeath;

    public delegate void EnemyArrivedInTown(BaseEnemy enemy);
    public static event EnemyArrivedInTown onEnemyArrivedInTown;

    public delegate void EnemiesAttakingTown();
    public static event EnemiesAttakingTown onEnemiesAttakingTown;

    public delegate void PlayerDied();
    public static event PlayerDied onPlayerDeath;

    public static void OnEnemyDeath(BaseEnemy enemy)
    {
        if(onEnemyDeath != null)
        {
            onEnemyDeath(enemy);
        }
    }

    public static void OnEnemyArrivedOnTown(BaseEnemy enemy)
    {
        if (onEnemyArrivedInTown != null)
        {
            onEnemyArrivedInTown(enemy);
        }
    }

    public static void OnEnemiesAttakingTown()
    {
        if (onEnemiesAttakingTown != null)
        {
            onEnemiesAttakingTown();
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