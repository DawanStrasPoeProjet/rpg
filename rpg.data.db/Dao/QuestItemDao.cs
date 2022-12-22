using Microsoft.EntityFrameworkCore;
using RPG.Data.Db.Contexts;

namespace RPG.Data.Db.Dao;

public class QuestItemDao : IItemDao<Models.QuestItem>
{
    private readonly RpgDbContext _ctx;

    public QuestItemDao(RpgDbContext ctx)
        => _ctx = ctx;

    public List<Models.QuestItem> GetItems()
        => _ctx.QuestItems.AsNoTracking().ToList();

    public Models.QuestItem? FindItemById(string id)
        => _ctx.QuestItems.FirstOrDefault(x => x.Id == id);


    public void SaveOrUpdateItem(Models.QuestItem item)
    {
        if (item.QuestItemId == 0)
            _ctx.QuestItems.Add(item);
        else
            _ctx.Entry(item).State = EntityState.Modified;
        _ctx.SaveChanges();
    }

    public void DeleteItem(Models.QuestItem item)
    {
        _ctx.QuestItems.Remove(item);
        _ctx.SaveChanges();
    }
}
