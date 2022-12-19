namespace RPG.Core;

public interface IEntity
{
    int Key { get; }
    string Id { get; }
    IEnumerable<string> Tags { get; }
    IBag Bag { get; }
    IItem EquippedItem { get; }
    string Name { get; }
    int Initiative { get; set; }
    int Health { get; set; }
    int MaxHealth { get; set; }
    int Magic { get; set; }
    int MaxMagic { get; set; }
    int Attack { get; set; }
    int Defense { get; set; }
    int Money { get; set; }
    bool IsPlayer { get; }

    bool HasTags(params string[] tags);
    bool HasTags(IEnumerable<string> tags);

    void TakeAllItemsFrom(IEntity entity);
    void Rename(string name);

    bool HasEquippedItem();
    IItem? GetEquippedItem();
    IItem GetEquippedItemOrDefault();
    IItem? TakeEquippedItem();
    bool RemoveEquippedItem();
    bool TakeAwayEquippedItem();
    IItem EquipNewItem(IItem item);
    IItem EquipNewItemById(string id);
    IItem EquipItemByKey(int key);
    IItem EquipFirstItemById(string id);
    IItem EquipFirstItemByTag(string tag);
    IItem EquipFirstItemByTags(IEnumerable<string> tags);
}
