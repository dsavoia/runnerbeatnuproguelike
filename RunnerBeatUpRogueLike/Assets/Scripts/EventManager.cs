using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour 
{
    public delegate void EnemyDied(BaseEnemy enemy);
    public static event EnemyDied onEnemyDeath;

    public delegate void EnemyArrivedInTown(BaseEnemy enemy);
    public static event EnemyArrivedInTown onEnemyArrivedInTown;

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
}