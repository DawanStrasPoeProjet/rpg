namespace RPG.Core;

public interface ICombatSystem
{
    IGame Game { get; set; }

    CombatResult BeginCombat(IEntity source, IEnumerable<IEntity> entities);
}
