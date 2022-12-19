namespace RPG.Core;

public interface IItemSource
{
    IItem Create(string id);
    string GetName(string id);
    string GetDescription(string id);
}
