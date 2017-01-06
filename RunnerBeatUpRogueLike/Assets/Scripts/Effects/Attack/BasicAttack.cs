class BasicAttack : IAttack
{

    protected int damage = 0;
    protected AttackTypes attackType = AttackTypes.Basic;

    public BasicAttack(int damage = 0) {
        AddDamage(damage);
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

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }
}