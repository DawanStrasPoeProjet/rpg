namespace RPG.Data.Database.Dao;

public interface IItemDAO<Item>
{
    List<Item> GetItems();
    Item? FindItemById(int id);
    void SaveOrUpdateItem(Item item);
    void DeleteItem(Item item);
}
