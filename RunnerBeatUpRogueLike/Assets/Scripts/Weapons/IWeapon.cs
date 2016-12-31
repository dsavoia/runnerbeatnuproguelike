using System.Collections.Generic;

public interface IWeapon
{
    int GetDamage();
    float GetAttackRate();
    void SetAttackRate(float attackRate);
    List<IAttack> Attack();
    void AddAttack(IAttack attack);
}

