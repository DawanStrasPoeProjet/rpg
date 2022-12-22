using RPG.Core;

namespace RPG.Data.Db;

public class CachedDbEntitySource : IEntitySource
{
    private int _key = 100_000;
    private readonly IItemSource _itemSource;
    private readonly IEntitySource _dbEntitySource;
    private readonly Dictionary<string, IEntity> _entities = new();

    public CachedDbEntitySource(IItemSource itemSource)
    {
        _itemSource = itemSource;
        _dbEntitySource = new DbEntitySource(itemSource);
    }

    public IEntity Create(string id)
    {
        if (_entities.ContainsKey(id))
        {
            var entity = _entities[id];
            string defaultEquippedItemId;

            if (entity.GetEquippedItem() is null)
                defaultEquippedItemId = entity.EquippedItem.Id;
            else
            {
                var equippedItem = entity.TakeEquippedItem()!;
                defaultEquippedItemId = entity.EquippedItem.Id;
                entity.Bag.AddItem(equippedItem);
                entity.EquipItemByKey(equippedItem.Key);
            }

            var e = new Entity(++_key, id, entity.Tags, _itemSource, defaultEquippedItemId)
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
            foreach (var i in entity.Bag.Items)
                e.Bag.AddItemById(i.Id);
            if (entity.GetEquippedItem() != null)
                e.EquipNewItemById(entity.GetEquippedItem()!.Id);
            return e;
        }
        else
        {
            var entity = _dbEntitySource.Create(id);
            _entities[id] = entity;
            return entity;
        }
    }
}
