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
    internal class WeaponItemDAO : IItemDAO<WeaponItem>
    {
        private RpgContext context;

        public WeaponItemDAO(RpgContext context)
        {
            this.context = context;
        }

        public async Task SaveOrUpdateItemAsync(WeaponItem item)
        {            
            if (item.WeaponId == 0)
            {
                context.WeaponItems.Add(item);
            }
            else
            {
                context.Entry(item).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(WeaponItem item)
        {
            context.WeaponItems.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<WeaponItem> FindItemByIdAsync(int id)
        {
            return await context.WeaponItems.FindAsync(id);
        }

        public async Task<List<WeaponItem>> GetItemsAsync()
        {
            return await context.WeaponItems.AsNoTracking().ToListAsync();
        }
    }
}
