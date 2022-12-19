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
            "fist" => new WeaponItem(++_key, id, Item.TagsOf("fast"), 0, 5),
            "sword" => new WeaponItem(++_key, id, Item.TagsOf("fast"), 0, 20),
            "axe" => new WeaponItem(++_key, id, Item.TagsOf("slow"), 0, 40),
            "small_life_potion" => new PotionItem(++_key, id, Item.TagsOf("healing"), 0, 20),
            "small_key" => new QuestItem(++_key, id, Item.TagsOf("key")),
            _ => throw new ArgumentException($"invalid item id `{id}`", nameof(id))
        };
    }
}
