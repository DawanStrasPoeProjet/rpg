namespace RPG.Core;

public interface IEntitySource
{
    IEntity Create(string id);
}
