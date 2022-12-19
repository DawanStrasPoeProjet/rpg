namespace RPG.Core;

public interface IInventorySystem
{
    IGame Game { get; set; }

    Item? PickItem(IEntity source);
}
