namespace RPG.Core;

public class Bag : IBag
{
    private readonly IItemSource _source;
    private readonly List<IItem> _items = new();
    public IEnumerable<IItem> Items => _items;

    public Bag(IItemSource source)
        => _source = source;

    public override string ToString()
        => $"{{{string.Join(", ", _items)}}}";

    public void Clear()
        => _items.Clear();

    public IItem AddItem(IItem item)
    {
        var dup = GetItemByKey(item.Key);
        if (dup != null)
            throw new BagException($"couldn't add duplicate item `{item}`");
        _items.Add(item);
        return item;
    }

    public void AddItems(IEnumerable<IItem> items)
    {
        foreach (var item in items)
            AddItem(item);
    }

    public IItem AddItemById(string id)
        => AddItem(_source.Create(id));

    public IEnumerable<IItem> AddItemsById(IEnumerable<string> ids)
        => ids.Select(AddItemById).ToList();

    public bool HasItemByKey(int key)
        => _items.FindIndex(x => x.Key == key) >= 0;

    public IItem? GetItemByKey(int key)
        => _items.Find(x => x.Key == key);

    public IItem? TakeItemByKey(int key)
    {
        var item = GetItemByKey(key);
        if (item != null)
            _items.Remove(item);
        return item;
    }

    public bool RemoveItemByKey(int key)
        => TakeItemByKey(key) != null;

    public void RemoveItemsByKey(IEnumerable<int> keys)
    {
        foreach (var key in keys)
            RemoveItemByKey(key);
    }

    public bool HasItemById(string id)
        => _items.FindIndex(x => x.Id == id) >= 0;

    public IItem? GetFirstItemById(string id)
        => _items.Find(x => x.Id == id);

    public IEnumerable<IItem> GetAllItemsById(string id)
        => _items.FindAll(x => x.Id == id);

    public IItem? TakeFirstItemById(string id)
    {
        var item = GetFirstItemById(id);
        if (item != null)
            _items.Remove(item);
        return item;
    }

    public IEnumerable<IItem> TakeAllItemsById(string id)
    {
        var items = GetAllItemsById(id).ToList();
        foreach (var item in items)
            _items.Remove(item);
        return items;
    }

    public bool RemoveFirstItemById(string id)
        => TakeFirstItemById(id) != null;

    public void RemoveAllItemsById(string id)
        => _items.RemoveAll(x => x.Id == id);

    public bool HasItemByTag(string tag)
        => _items.FindIndex(x => x.Tags.Contains(tag)) >= 0;

    public IItem? GetFirstItemByTag(string tag)
        => _items.Find(x => x.Tags.Contains(tag));

    public IEnumerable<IItem> GetAllItemsByTag(string tag)
        => _items.FindAll(x => x.Tags.Contains(tag));

    public IItem? TakeFirstItemByTag(string tag)
    {
        var item = GetFirstItemByTag(tag);
        if (item != null)
            _items.Remove(item);
        return item;
    }

    public IEnumerable<IItem> TakeAllItemsByTag(string tag)
    {
        var items = GetAllItemsByTag(tag).ToList();
        foreach (var item in items)
            _items.Remove(item);
        return items;
    }

    public bool RemoveFirstItemByTag(string tag)
        => TakeFirstItemByTag(tag) != null;

    public void RemoveAllItemsByTag(string tag)
        => _items.RemoveAll(x => x.Tags.Contains(tag));

    public bool HasItemByTags(IEnumerable<string> tags)
        => _items.FindIndex(x => tags.All(x.Tags.Contains)) >= 0;

    public IItem? GetFirstItemByTags(IEnumerable<string> tags)
        => _items.Find(x => tags.All(x.Tags.Contains));

    public IEnumerable<IItem> GetAllItemsByTags(IEnumerable<string> tags)
        => _items.FindAll(x => tags.All(x.Tags.Contains));

    public IItem? TakeFirstItemByTags(IEnumerable<string> tags)
    {
        var item = GetFirstItemByTags(tags);
        if (item != null)
            _items.Remove(item);
        return item;
    }

    public IEnumerable<IItem> TakeAllItemsByTags(IEnumerable<string> tags)
    {
        var items = GetAllItemsByTags(tags).ToList();
        foreach (var item in items)
            _items.Remove(item);
        return items;
    }

    public bool RemoveFirstItemByTags(IEnumerable<string> tags)
        => TakeFirstItemByTags(tags) != null;

    public void RemoveAllItemsByTags(IEnumerable<string> tags)
        => _items.RemoveAll(x => tags.All(x.Tags.Contains));
}
