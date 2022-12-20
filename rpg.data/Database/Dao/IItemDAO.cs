using RpgAppDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAppDatabase.Dao
{
    internal interface IItemDAO<Item>
    {
        Task<List<Item>> GetItemsAsync();
        Task<Item> FindItemByIdAsync(int id);
        Task SaveOrUpdateItemAsync(Item item);
        Task DeleteItemAsync(Item item);
    }
}
