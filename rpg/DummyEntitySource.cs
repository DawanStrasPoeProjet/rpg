using RPG.Core;

namespace RPG;

public class DummyEntitySource : IEntitySource
{
    private int _key;
    private readonly IItemSource _itemSource;

    public DummyEntitySource(IItemSource itemSource)
        => _itemSource = itemSource;

    public IEntity Create(string id)
    {
        return id switch
        {
            "player" => new Entity(++_key, id, Entity.TagsOf("player"), _itemSource, "fist")
                { IsPlayer = true, Health = 100, MaxHealth = 100 },
            "swordsman" => new Entity(++_key, id, null, _itemSource, "sword")
                { Health = 80, MaxHealth = 80 },
            _ => throw new ArgumentException($"invalid entity id `{id}`", nameof(id))
        };
    }
}
