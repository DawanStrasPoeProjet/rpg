namespace RPG.Data.Db.Dao;

public interface IItemDao<TItem>
{
    List<TItem> GetItems();
    TItem? FindItemById(string id);
    void SaveOrUpdateItem(TItem item);
    void DeleteItem(TItem item);
}
