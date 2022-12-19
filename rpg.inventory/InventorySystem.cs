using RPG.Core;

namespace RPG.Inventory;

public class InventorySystem : IInventorySystem
{
    public IGame Game { get; set; } = null!;

    public Item? PickItem(IEntity source)
    {
        throw new NotImplementedException();
    }
}
