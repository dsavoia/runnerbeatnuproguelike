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
        Vector2 targetPosition = playerInfo.targetObject.GetComponent<BoxCollider2D>().bounds.center;
                
        Debug.DrawLine(transform.position, targetPosition, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, playerInfo.interactiveObjectsLayer);

        if (hit.distance > playerInfo.basicAttackRange)
        {
            playerInfo.SetState(PlayerInfo.PlayerState.MovingToTarget);
            return;
        }

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
