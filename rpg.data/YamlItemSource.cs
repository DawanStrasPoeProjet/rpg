using RPG.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RPG.Data;

public class YamlItemSource : IItemSource
{
    private int _key;
    private readonly Dictionary<string, ItemDto> _items = new();

    public YamlItemSource(string path)
        => LoadYaml(path);

    private void LoadYaml(string path)
    {
        using var reader = new StreamReader(path);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping("!WeaponItem", typeof(WeaponItemDto))
            .WithTagMapping("!PotionItem", typeof(PotionItemDto))
            .WithTagMapping("!QuestItem", typeof(QuestItemDto))
            .Build();
        var items = deserializer.Deserialize<ItemDto[]>(reader);
        foreach (var item in items)
        {
            item.Tags ??= new List<string>();
            item.Name ??= string.Empty;
            item.Description ??= string.Empty;
            _items.Add(item.Id, item);
        }
    }

    public IItem Create(string id)
    {
        var item = _items[id];
        return item switch
        {
            WeaponItemDto dto => new WeaponItem(++_key, id, dto.Name, dto.Description, dto.Tags, dto.Price,
                dto.BaseDamage, dto.NumDiceRolls, dto.NumDiceFaces),
            PotionItemDto dto => new PotionItem(++_key, id, dto.Name, dto.Description, dto.Tags, dto.Price, dto.Health,
                dto.Magic),
            QuestItemDto dto => new QuestItem(++_key, id, dto.Name, dto.Description, dto.Tags, dto.Price),
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }

    public string GetName(string id)
        => _items[id].Name;

    public string GetDescription(string id)
        => _items[id].Description;
}
