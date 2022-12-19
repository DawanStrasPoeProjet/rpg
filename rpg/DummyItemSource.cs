using RPG.Core;
using RPG.Data;

namespace RPG;

public class DummyItemSource : IItemSource
{
    private int _key;

    public IItem Create(string id)
    {
        return id switch
        {
            "fist" => new WeaponItem(++_key, id, GetName(id), GetDescription(id), Item.TagsOf("fast"), 0, 5),
            "sword" => new WeaponItem(++_key, id, GetName(id), GetDescription(id), Item.TagsOf("fast"), 0, 20),
            "axe" => new WeaponItem(++_key, id, GetName(id), GetDescription(id), Item.TagsOf("slow"), 0, 40),
            "small_life_potion" => new PotionItem(++_key, id, GetName(id), GetDescription(id), Item.TagsOf("healing"),
                0, 20),
            "small_key" => new QuestItem(++_key, id, GetName(id), GetDescription(id), Item.TagsOf("key")),
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }

    public string GetName(string id)
    {
        return id switch
        {
            "fist" => "Poings",
            "sword" => "Épée",
            "axe" => "Hache",
            "small_life_potion" => "Petite potion de vie",
            "small_key" => "Petite clé",
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }

    public string GetDescription(string id)
    {
        return id switch
        {
            "fist" => "Des poings...",
            "sword" => "Une épée...",
            "axe" => "Une hache...",
            "small_life_potion" => "Une petite potion de vie...",
            "small_key" => "Une petite clé...",
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }
}
