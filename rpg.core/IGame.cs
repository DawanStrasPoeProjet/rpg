namespace RPG.Core;

public interface IGame
{
    IItemSource ItemSource { get; }
    IEntitySource EntitySource { get; }
    IInventorySystem InventorySystem { get; }
    ICombatSystem CombatSystem { get; }
    IStageSystem StageSystem { get; }

    bool HasFlag(string name);
    bool GetFlag(string name);
    void SetFlag(string name, bool value);
    bool HasData(string name);
    object GetData(string name);
    void SetData(string name, object value);
}
