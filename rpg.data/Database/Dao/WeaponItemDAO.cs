using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class WeaponItemDAO : IItemDAO<Model.WeaponItem>
{
    private RpgContext context;

    public WeaponItemDAO()
    {
        context = new RpgContext();
    }   

    public List<Model.WeaponItem> GetItems()
    {
        return context.WeaponItems.AsNoTracking().ToList();
    }

    public Model.WeaponItem? FindItemById(string id)
    {
        return context.WeaponItems.Find(id);
    }

    public void SaveOrUpdateItem(Model.WeaponItem item)
    {
        if (item.WeaponId == 0)
        {
            context.WeaponItems.Add(item);
        }
        else
        {
            context.Entry(item).State = EntityState.Modified;
        }

        context.SaveChangesAsync();
    }

    public void DeleteItem(Model.WeaponItem item)
    {
        context.WeaponItems.Remove(item);
        context.SaveChanges();
    }
}
