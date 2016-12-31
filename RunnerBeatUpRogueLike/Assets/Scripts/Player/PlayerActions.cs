using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerActions : MonoBehaviour, ICombatTarget, IAttacker {

    float attackCoolDown;

    void Start()
    {
        attackCoolDown = 1 / PlayerInfo.instance.attackRate;
    }

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

        if (hit.distance > PlayerInfo.instance.attackRange)
        {
            PlayerInfo.instance.SetState(PlayerInfo.PlayerState.MovingToTarget);
            return;
        }

        if (Time.time > PlayerInfo.instance.lastAttackTime + attackCoolDown)
        {
            PlayerInfo.instance.isBasicAttackOnCooldown = false;
            //PlayerInfo.instance.focusedEnemy.TakeDamage(PlayerInfo.instance.attackDamage);            
            GetTarget().Defend(Attack());
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

    public List<IAttack> Attack()
    {
        List<IAttack> attacks = new List<IAttack>();
        attacks.Add(PlayerInfo.instance.basicAttack);
        attacks.AddRange(PlayerInfo.instance.weapon.Attack());
        return attacks;
    }

    public ICombatTarget GetTarget()
    {
        return PlayerInfo.instance.focusedEnemy;
    }

    public void Defend(List<IAttack> attacks)
    {
        int damage = 0;
        foreach (IAttack attack in attacks)
        {
            damage += attack.GetDamage();
        }
        TakeDamage(damage);
    }

    
}
