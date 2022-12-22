using RPG.Core;
using RPG.Data.Db.Contexts;
using RPG.Data.Db.Dao;

namespace RPG.Data.Db;

public class DbEntitySource : IEntitySource
{
    private int _key;
    private readonly IItemSource _itemSource;

    public DbEntitySource(IItemSource itemSource)
        => _itemSource = itemSource;

    public IEntity Create(string id)
    {
        using var ctx = new RpgDbContext();
        var dao = new EntityDao(ctx);
        var entityModel = dao.FindEntityById(id);

        if (entityModel is null)
            throw new Exception($"entity with id={id} not found");

        entityModel.Tags ??= string.Empty;
        entityModel.BagItemIds ??= string.Empty;
        entityModel.Name ??= string.Empty;

        var tags = entityModel.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var bagItemIds = entityModel.BagItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

        var e = new Entity(++_key, id, tags, _itemSource, entityModel.DefaultEquippedItemId)
        {
            Initiative = entityModel.Initiative,
            Health = entityModel.Health,
            MaxHealth = entityModel.MaxHealth,
            Magic = entityModel.Magic,
            MaxMagic = entityModel.MaxMagic,
            Attack = entityModel.Attack,
            Defense = entityModel.Defense,
            Money = entityModel.Money,
            IsPlayer = entityModel.IsPlayer
        };
        e.Rename(entityModel.Name);
        foreach (var i in bagItemIds)
            e.Bag.AddItemById(i);
        if (entityModel.EquippedItemId != null)
            e.EquipNewItemById(entityModel.EquippedItemId);
        return e;
    }
}
