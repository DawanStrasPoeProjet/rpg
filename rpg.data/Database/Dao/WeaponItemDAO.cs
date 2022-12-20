using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class WeaponItemDAO : IItemDAO<Model.WeaponItem>
{
    private RpgContext context;

    public WeaponItemDAO(RpgContext context)
    {
        this.context = context;
    }

    public async Task SaveOrUpdateItemAsync(Model.WeaponItem item)
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

    public async Task DeleteItemAsync(Model.WeaponItem item)
    {
        context.WeaponItems.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<Model.WeaponItem> FindItemByIdAsync(int id)
    {
        return await context.WeaponItems.FindAsync(id);
    }

    public async Task<List<Model.WeaponItem>> GetItemsAsync()
    {
        return await context.WeaponItems.AsNoTracking().ToListAsync();
    }
}
