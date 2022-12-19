namespace RPG.Core;

public interface IBag
{
    IEnumerable<IItem> Items { get; }

    void Clear();

    IItem AddItem(IItem item);
    void AddItems(IEnumerable<IItem> items);
    IItem AddItemById(string id);
    IEnumerable<IItem> AddItemsById(IEnumerable<string> ids);

    bool HasItemByKey(int key);
    IItem? GetItemByKey(int key);
    IItem? TakeItemByKey(int key);
    bool RemoveItemByKey(int key);
    void RemoveItemsByKey(IEnumerable<int> keys);

    bool HasItemById(string id);
    IItem? GetFirstItemById(string id);
    IEnumerable<IItem> GetAllItemsById(string id);
    IItem? TakeFirstItemById(string id);
    IEnumerable<IItem> TakeAllItemsById(string id);
    bool RemoveFirstItemById(string id);
    void RemoveAllItemsById(string id);

    bool HasItemByTag(string tag);
    IItem? GetFirstItemByTag(string tag);
    IEnumerable<IItem> GetAllItemsByTag(string tag);
    IItem? TakeFirstItemByTag(string tag);
    IEnumerable<IItem> TakeAllItemsByTag(string tag);
    bool RemoveFirstItemByTag(string tag);
    void RemoveAllItemsByTag(string tag);

    bool HasItemByTags(IEnumerable<string> tags);
    IItem? GetFirstItemByTags(IEnumerable<string> tags);
    IEnumerable<IItem> GetAllItemsByTags(IEnumerable<string> tags);
    IItem? TakeFirstItemByTags(IEnumerable<string> tags);
    IEnumerable<IItem> TakeAllItemsByTags(IEnumerable<string> tags);
    bool RemoveFirstItemByTags(IEnumerable<string> tags);
    void RemoveAllItemsByTags(IEnumerable<string> tags);
}
