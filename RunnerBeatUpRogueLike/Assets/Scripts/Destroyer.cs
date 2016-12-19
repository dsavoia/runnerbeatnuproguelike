using UnityEngine;

public class Destroyer : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            PutEnemyOnTown(other.GetComponent<BaseEnemy>());
        }
    }

    void PutEnemyOnTown(BaseEnemy enemy)
    {
        EventManager.OnEnemyArrivedOnTown(enemy);
        Destroy(enemy.gameObject);
    }    
}