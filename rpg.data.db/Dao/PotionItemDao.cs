using Microsoft.EntityFrameworkCore;
using RPG.Data.Db.Contexts;

namespace RPG.Data.Db.Dao;

public class PotionItemDao : IItemDao<Models.PotionItem>
{
    private readonly RpgDbContext _ctx;

    public PotionItemDao(RpgDbContext ctx)
        => _ctx = ctx;

    public List<Models.PotionItem> GetItems()
        => _ctx.PotionItems.AsNoTracking().ToList();

    public Models.PotionItem? FindItemById(string id)
        => _ctx.PotionItems.FirstOrDefault(x => x.Id == id);

    public void SaveOrUpdateItem(Models.PotionItem item)
    {
        if (item.PotionItemId == 0)
            _ctx.PotionItems.Add(item);
        else
            _ctx.Entry(item).State = EntityState.Modified;
        _ctx.SaveChanges();
    }

    public void DeleteItem(Models.PotionItem item)
    {
        _ctx.PotionItems.Remove(item);
        _ctx.SaveChanges();
    }
}
