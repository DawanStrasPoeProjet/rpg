using Microsoft.EntityFrameworkCore;
using RPG.Data.Db.Contexts;

namespace RPG.Data.Db.Dao;

public class WeaponItemDao : IItemDao<Models.WeaponItem>
{
    private readonly RpgDbContext _ctx;

    public WeaponItemDao(RpgDbContext ctx)
        => _ctx = ctx;

    public List<Models.WeaponItem> GetItems()
        => _ctx.WeaponItems.AsNoTracking().ToList();

    public Models.WeaponItem? FindItemById(string id)
        => _ctx.WeaponItems.FirstOrDefault(x => x.Id == id);

    public void SaveOrUpdateItem(Models.WeaponItem item)
    {
        if (item.WeaponItemId == 0)
            _ctx.WeaponItems.Add(item);
        else
            _ctx.Entry(item).State = EntityState.Modified;
        _ctx.SaveChangesAsync();
    }

    public void DeleteItem(Models.WeaponItem item)
    {
        _ctx.WeaponItems.Remove(item);
        _ctx.SaveChanges();
    }
}
