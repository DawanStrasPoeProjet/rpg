namespace RPG.Core;

public interface IEntity
{
    int Key { get; }
    string Id { get; }
    IEnumerable<string> Tags { get; }
    IBag Bag { get; }
    IItem EquippedItem { get; }
    public string Name { get; }
    public int Initiative { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Magic { get; set; }
    public int MaxMagic { get; set; }
    public int Defense { get; set; }
    public int Money { get; set; }
    public bool IsPlayer { get; }

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
