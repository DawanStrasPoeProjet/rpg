namespace RPG.Core;

public class Game : IGame
{
    public IItemSource ItemSource { get; }
    public IEntitySource EntitySource { get; }
    public IInventorySystem InventorySystem { get; }
    public ICombatSystem CombatSystem { get; }

    private readonly Dictionary<string, bool> _flags = new();
    private readonly Dictionary<string, object> _data = new();

    public Game(IItemSource itemSource, IEntitySource entitySource, IInventorySystem inventorySystem,
        ICombatSystem combatSystem)
    {
        ItemSource = itemSource;
        EntitySource = entitySource;
        InventorySystem = inventorySystem;
        InventorySystem.Game = this;
        CombatSystem = combatSystem;
        CombatSystem.Game = this;
    }

    public bool HasFlag(string name)
        => _flags.ContainsKey(name);

    public bool GetFlag(string name)
        => _flags[name];

    public void SetFlag(string name, bool value)
        => _flags[name] = value;

    public bool HasData(string name)
        => _data.ContainsKey(name);

    public object GetData(string name)
        => _data[name];

    public void SetData(string name, object value)
        => _data[name] = value;
}
