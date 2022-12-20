namespace RPG.Data.Database.Dao;

internal interface IItemDAO<Item>
{
    Task<List<Item>> GetItemsAsync();
    Task<Item> FindItemByIdAsync(int id);
    Task SaveOrUpdateItemAsync(Item item);
    Task DeleteItemAsync(Item item);
}
