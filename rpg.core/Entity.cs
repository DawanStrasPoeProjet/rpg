namespace RPG.Core;

public class Entity : IEntity
{
    public int Key { get; }
    public string Id { get; }
    private readonly List<string> _tags = new();
    public IEnumerable<string> Tags => _tags;
    public IBag Bag { get; }
    private readonly IItem _defaultEquippedItem;
    private IItem? _equippedItem;
    public IItem EquippedItem => GetEquippedItemOrDefault();
    public string Name { get; private set; } = string.Empty;
    public int Initiative { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Magic { get; set; }
    public int MaxMagic { get; set; }
    public int Defense { get; set; }
    public bool IsPlayer { get; init; }

    public Entity(int key, string id, IEnumerable<string>? tags, IItemSource itemSource, string defaultEquippedItemId)
    {
        Key = key;
        Id = id;
        if (tags != null) _tags.AddRange(tags);
        Bag = new Bag(itemSource);
        _defaultEquippedItem = itemSource.Create(defaultEquippedItemId);
    }

    public override string ToString()
        => $"Entity({nameof(Key)}={Key}, {nameof(Id)}={Id}, {nameof(Tags)}={{{string.Join(", ", Tags)}}}" +
           $", {nameof(Bag)}={Bag}" +
           $", DefaultEquippedItem={_defaultEquippedItem}" +
           $", EquippedItem={_equippedItem}" +
           $", {nameof(Name)}={Name}" +
           $", {nameof(Initiative)}={Initiative}" +
           $", {nameof(Health)}={Health}" +
           $", {nameof(MaxHealth)}={MaxHealth}" +
           $", {nameof(Magic)}={Magic}" +
           $", {nameof(MaxMagic)}={MaxMagic}" +
           $", {nameof(Defense)}={Defense}" +
           $", {nameof(IsPlayer)}={IsPlayer})";

    protected void InsertFrontTag(string tag)
        => _tags.Insert(0, tag);

    public static IEnumerable<string> TagsOf(params string[] tags)
        => tags;

    public bool HasTags(params string[] tags)
        => tags.All(Tags.Contains);

    public bool HasTags(IEnumerable<string> tags)
        => tags.All(Tags.Contains);

    public void TakeAllItemsFrom(IEntity entity)
    {
        Bag.AddItems(entity.Bag.Items);
        entity.Bag.Clear();
    }

    public void Rename(string name)
        => Name = name;

    public bool HasEquippedItem()
        => _equippedItem != null;

    public IItem? GetEquippedItem()
        => _equippedItem;

    public IItem GetEquippedItemOrDefault()
        => _equippedItem ?? _defaultEquippedItem;

    public IItem? TakeEquippedItem()
    {
        var item = _equippedItem;
        _equippedItem = null;
        return item;
    }

    public bool RemoveEquippedItem()
        => TakeEquippedItem() != null;

    public bool TakeAwayEquippedItem()
    {
        if (!HasEquippedItem()) return false;
        Bag.AddItem(TakeEquippedItem()!);
        return true;
    }

    public IItem EquipNewItem(IItem item)
        => EquipItemByKey(Bag.AddItem(item).Key);

    public IItem EquipNewItemById(string id)
        => EquipItemByKey(Bag.AddItemById(id).Key);

    private IItem EquipItem(IItem item)
    {
        TakeAwayEquippedItem();
        Bag.RemoveItemByKey(item.Key);
        _equippedItem = item;
        return item;
    }

    public IItem EquipItemByKey(int key)
    {
        var item = Bag.GetItemByKey(key);
        if (item is null)
            throw new BagException($"couldn't equip non-existent item with key `{key}`");
        return EquipItem(item);
    }

    public IItem EquipFirstItemById(string id)
    {
        var item = Bag.GetFirstItemById(id);
        if (item is null)
            throw new BagException($"couldn't equip non-existent item with id `{id}`");
        return EquipItem(item);
    }

    public IItem EquipFirstItemByTag(string tag)
    {
        var item = Bag.GetFirstItemByTag(tag);
        if (item is null)
            throw new BagException($"couldn't equip non-existent item with tag `{tag}`");
        return EquipItem(item);
    }

    public IItem EquipFirstItemByTags(IEnumerable<string> tags)
    {
        var tagsList = tags.ToList();
        var item = Bag.GetFirstItemByTags(tagsList);
        if (item is null)
            throw new BagException($"couldn't equip non-existent item with tags `{{{string.Join(", ", tagsList)}}}`");
        return EquipItem(item);
    }
}
