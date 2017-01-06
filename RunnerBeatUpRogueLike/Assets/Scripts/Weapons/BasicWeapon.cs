using System;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour, IWeapon {

    [SerializeField]
    private string displayName;
    [SerializeField]
    private float atkRate = 0.5f;
    [SerializeField]
    private int baseDamage = 1;

    List<IAttack> attacks = new List<IAttack>();

    public void SetAttackDamage(int weaponDamage)
    {
        baseDamage = weaponDamage;
        attacks = new List<IAttack>();
        AddAttack(new BasicAttack(weaponDamage));
    }

    public string GetName(){
        return displayName;
    }

    public void SetName(string name)
    {
        this.displayName = name;
    }

    public int GetDamage()
    {
        int damage = 0;
        foreach (IAttack attack in Attack())
        {
            damage += attack.GetDamage();
        }
        return damage;
    }

    public float GetAttackRate()
    {
        return atkRate;
    }

    public void SetAttackRate(float attackRate)
    {
        this.atkRate = attackRate;
    }

    public List<IAttack> Attack()
    {
        return attacks;
    }

    public void AddAttack(IAttack attack)
    {
        attacks.Add(attack);
    }
}
