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
    internal class QuestItemDAO : IItemDAO<QuestItem>
    {
        private RpgContext context;

        public QuestItemDAO(RpgContext context)
        {
            this.context = context;
        }

        public async Task SaveOrUpdateItemAsync(QuestItem item)
        {
            if (item.QuestId == 0)
            {
                context.QuestItems.Add(item);
            }
            else
            {
                context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(QuestItem item)
        {
            context.QuestItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<QuestItem> FindItemByIdAsync(int id)
        {
            return await context.QuestItems.FindAsync(id);
        }

        public async Task<List<QuestItem>> GetItemsAsync()
        {
            return await context.QuestItems.AsNoTracking().ToListAsync();
        }
    }
}
