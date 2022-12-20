using Microsoft.EntityFrameworkCore;
using RpgAppDatabase.Context;
using RpgAppDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAppDatabase.Dao
{
    internal class PotionItemDAO : IItemDAO<PotionItem>
    {
        private RpgContext context;

        public PotionItemDAO(RpgContext context)
        {
            this.context = context;
        }

        public async Task SaveOrUpdateItemAsync(PotionItem item)
        {
            if (item.PotionId == 0)
            {
                context.PotionItems.Add(item);
            }
            else
            {
                context.PotionItems.Add(item);
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(PotionItem item)
        {
            context.PotionItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<PotionItem> FindItemByIdAsync(int id)
        {
            return await context.PotionItems.FindAsync(id);
        }

        public async Task<List<PotionItem>> GetItemsAsync()
        {
            return await context.PotionItems.AsNoTracking().ToListAsync();
        }
    }
}
