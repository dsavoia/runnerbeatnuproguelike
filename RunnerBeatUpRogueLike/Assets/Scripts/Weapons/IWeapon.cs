using System.Collections.Generic;

public interface IWeapon
{
    int GetDamage();
    string GetName();
    void SetAttackDamage(int weaponDamage);
    float GetAttackRate();
    void SetAttackRate(float attackRate);
    List<IAttack> Attack();
    void AddAttack(IAttack attack);
}

