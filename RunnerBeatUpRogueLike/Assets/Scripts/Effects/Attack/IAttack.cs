public interface IAttack
{
    int GetDamage();
    void AddDamage(int damage);
    AttackTypes GetAttackType();
    void SetAttackType(AttackTypes attack);
}

