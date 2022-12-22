namespace RPG.Data.Database.Dao;

public interface IItemDAO<Item>
{
    List<Item> GetItems();
    Item? FindItemById(string id);
    void SaveOrUpdateItem(Item item);
    void DeleteItem(Item item);
}
