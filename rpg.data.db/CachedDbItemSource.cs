using RPG.Core;

namespace RPG.Data.Db;

public class CachedDbItemSource : IItemSource
{
    private int _key = 100_000;
    private readonly IItemSource _dbItemSource = new DbItemSource();
    private readonly Dictionary<string, IItem> _items = new();
    private readonly Dictionary<string, string> _names = new();
    private readonly Dictionary<string, string> _descriptions = new();

    public IItem Create(string id)
    {
        if (_items.ContainsKey(id))
        {
            return _items[id] switch
            {
                WeaponItem weaponItem => new WeaponItem(++_key, id, weaponItem.Name, weaponItem.Description,
                    weaponItem.Tags, weaponItem.Price, weaponItem.BaseDamage, weaponItem.NumDiceRolls,
                    weaponItem.NumDiceFaces),
                PotionItem potionItem => new PotionItem(++_key, id, potionItem.Name, potionItem.Description,
                    potionItem.Tags, potionItem.Price, potionItem.Health, potionItem.Magic),
                QuestItem questItem => new QuestItem(++_key, id, questItem.Name, questItem.Description, questItem.Tags,
                    questItem.Price),
                _ => throw new Exception($"item with id={id} not found")
            };
        }

        var item = _dbItemSource.Create(id);
        _items[id] = item;
        return item;
    }

    public string GetName(string id)
    {
        if (_names.ContainsKey(id))
            return _names[id];

        var name = _dbItemSource.GetName(id);
        _names[id] = name;
        return name;
    }

    public string GetDescription(string id)
    {
        if (_descriptions.ContainsKey(id))
            return _descriptions[id];

        var description = _dbItemSource.GetDescription(id);
        _descriptions[id] = description;
        return description;
    }
}
