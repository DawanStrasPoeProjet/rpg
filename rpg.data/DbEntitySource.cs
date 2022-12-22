using RPG.Core;

namespace RPG.Data;

public class DbEntitySource : IEntitySource
{
    private int _key;
    private readonly IItemSource _itemSource;

    public DbEntitySource(IItemSource itemSource)
    {
        _itemSource = itemSource;
    }

    public IEntity Create(string id)
    {        
        var dao = new Database.Dao.EntityDAO();
        Database.Model.Entity entity = dao.FindEntityById(id);
        var e = new Entity(++_key, id, entity.Tags.Split(','), _itemSource , entity.DefaultEquippedItemId)
        {
            Initiative = entity.Initiative,
            Health = entity.Health,
            MaxHealth = entity.MaxHealth,
            Magic = entity.Magic,
            MaxMagic = entity.MaxMagic,
            Attack = entity.Attack,
            Defense = entity.Defense,
            Money = entity.Money,
            IsPlayer = entity.IsPlayer

        };
        e.Rename(entity.Name);
        if (entity.BagItemIds != null)
            foreach (var i in entity.BagItemIds)
                e.Bag.AddItemById(i.ToString());
        if (entity.EquippedItemId != null)
            e.EquipNewItemById(entity.EquippedItemId);
        return e;

    }

}
