using RPG.Core;

namespace RPG.Combat;

public class CombatSystem : ICombatSystem
{
    public IGame Game { get; set; } = null!;

    public CombatResult BeginCombat(IEntity source, IEnumerable<IEntity> entities)
    {
        throw new NotImplementedException();
    }
}
