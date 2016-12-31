using System.Collections.Generic;

public interface ICombatTarget
{
    void Defend(List<IAttack> attacks);
}

