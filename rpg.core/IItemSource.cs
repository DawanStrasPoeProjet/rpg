namespace RPG.Core;

public interface IItemSource
{
    IItem Create(string id);
}
