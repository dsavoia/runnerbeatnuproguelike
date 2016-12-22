using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {

    PlayerInfo playerInfo;

	// Use this for initialization
	void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();
	}

    void Update()
    {
        switch (playerInfo.state)
        {
            case (PlayerInfo.PlayerState.Fighting):
                Fight();
            break;
        }
    }

    protected virtual void Fight()
    {
        /*Debug.DrawLine(enemyBounds.center, player.transform.position, Color.yellow);
        RaycastHit2D hit = Physics2D.Linecast(enemyBounds.center, player.transform.position, viewLayer);
        
        if (hit.distance > attackRange)
        {
            state = BaseEnemyState.MovingToPlayer;
            return;
        }*/

        if (Time.time > playerInfo.lastAttackTime + playerInfo.basicAttackCooldown)
        {
            playerInfo.isBasicAttackOnCooldown = false;
            playerInfo.focusedEnemy.TakeDamage(playerInfo.basicAttackDamage);            
            playerInfo.lastAttackTime = Time.time;
        }
    }    

    public void TakeDamage(int damage)
    {
        playerInfo.currentHp -= damage;
        if (playerInfo.currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerInfo.SetState(PlayerInfo.PlayerState.Dead);
        EventManager.OnPlayerDeath();
    }
}
