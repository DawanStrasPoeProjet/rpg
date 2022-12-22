using RPG.Core;

namespace RPG.Combat.UI;

public interface IUICombat
{
    void WaitEnterKeyPress();
    void Display();

    void Update(IEntity? player = null,
        IEnumerable<IEntity>? turnOrder = null,
        IEnumerable<IEntity>? aliveEntities = null,
        IEnumerable<IEntity>? enemies = null,
        string? description = null,
        int turn = -1);

    dynamic? GetPrompt(PromptType prompt);
}
