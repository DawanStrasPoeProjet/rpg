namespace RPG.Core;

public interface IInventorySystem
{
    IGame Game { get; set; }

    IItem? PickItem(IEntity source);
}
