﻿using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {    	

    void Update()
    {
        switch (PlayerInfo.instance.state)
        {
            case (PlayerInfo.PlayerState.Fighting):
                Fight();
            break;
        }
    }

    protected virtual void Fight()
    {
        Vector2 targetPosition = PlayerInfo.instance.targetObject.GetComponent<BoxCollider2D>().bounds.center;
                
        Debug.DrawLine(transform.position, targetPosition, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPosition, PlayerInfo.instance.interactiveObjectsLayer);

        if (hit.distance > PlayerInfo.instance.basicAttackRange)
        {
            PlayerInfo.instance.SetState(PlayerInfo.PlayerState.MovingToTarget);
            return;
        }

        if (Time.time > PlayerInfo.instance.lastAttackTime + PlayerInfo.instance.basicAttackCooldown)
        {
            PlayerInfo.instance.isBasicAttackOnCooldown = false;
            PlayerInfo.instance.focusedEnemy.TakeDamage(PlayerInfo.instance.basicAttackDamage);            
            PlayerInfo.instance.lastAttackTime = Time.time;
        }
    }    

    public void TakeDamage(int damage)
    {
        PlayerInfo.instance.currentHp -= damage;
        if (PlayerInfo.instance.currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerInfo.instance.SetState(PlayerInfo.PlayerState.Dead);
        EventManager.OnPlayerDeath();
    }
}
