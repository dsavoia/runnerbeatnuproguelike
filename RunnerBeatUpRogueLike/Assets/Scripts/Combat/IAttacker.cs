using System.Collections.Generic;

public interface IAttacker
{
    List<IAttack> Attack();
    ICombatTarget GetTarget();
}

