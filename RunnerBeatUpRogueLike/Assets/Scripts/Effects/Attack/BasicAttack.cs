class BasicAttack : IAttack
{

    protected int damage = 0;
    protected AttackTypes attackType = AttackTypes.Basic;

    public BasicAttack(int damage = 0) {
        SetDamage(damage);
    }

    public AttackTypes GetAttackType()
    {
        return attackType;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetAttackType(AttackTypes attack)
    {
        this.attackType = attack;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}