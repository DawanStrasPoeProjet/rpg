using RPG.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RPG.Data;

public class YamlEntitySource : IEntitySource
{
    private int _key;
    private readonly IItemSource _itemSource;
    private readonly Dictionary<string, EntityDto> _entities = new();

    public YamlEntitySource(IItemSource itemSource, string path)
    {
        _itemSource = itemSource;
        LoadYaml(path);
    }

    private void LoadYaml(string path)
    {
        using var reader = new StreamReader(path);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var entities = deserializer.Deserialize<EntityDto[]>(reader);
        foreach (var entity in entities)
        {
            entity.Tags ??= new List<string>();
            entity.Name ??= string.Empty;
            _entities.Add(entity.Id, entity);
        }
    }

    public IEntity Create(string id)
    {
        var dto = _entities[id];
        var e = new Entity(++_key, id, dto.Tags, _itemSource, dto.DefaultEquippedItemId)
        {
            Initiative = dto.Initiative,
            Health = dto.Health,
            MaxHealth = dto.MaxHealth,
            Magic = dto.Magic,
            MaxMagic = dto.MaxMagic,
            Attack = dto.Attack,
            Defense = dto.Defense,
            Money = dto.Money,
            IsPlayer = dto.IsPlayer
        };
        e.Rename(dto.Name);
        if (dto.BagItemIds != null)
            foreach (var i in dto.BagItemIds)
                e.Bag.AddItemById(i);
        if (dto.EquippedItemId != null)
            e.EquipNewItemById(dto.EquippedItemId);
        return e;
    }
}
