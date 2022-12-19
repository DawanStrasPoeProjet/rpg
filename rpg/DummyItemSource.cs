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
            "fist" => new WeaponItem(++_key, id, Item.TagsOf("fast"), 5, 0, 0),
            "sword" => new WeaponItem(++_key, id, Item.TagsOf("fast"), 20, 0, 0),
            "axe" => new WeaponItem(++_key, id, Item.TagsOf("slow"), 20, 0, 0),
            "small_life_potion" => new PotionItem(++_key, id, Item.TagsOf("healing"), 20, 0),
            "small_key" => new QuestItem(++_key, id, Item.TagsOf("key")),
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }
}
