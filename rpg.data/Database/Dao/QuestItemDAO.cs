using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class QuestItemDAO : IItemDAO<Model.QuestItem>
{
    private RpgContext context;

    public QuestItemDAO(RpgContext context)
    {
        this.context = context;
    }

    public async Task SaveOrUpdateItemAsync(Model.QuestItem item)
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

    public async Task DeleteItemAsync(Model.QuestItem item)
    {
        context.QuestItems.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<Model.QuestItem> FindItemByIdAsync(int id)
    {
        return await context.QuestItems.FindAsync(id);
    }

    public async Task<List<Model.QuestItem>> GetItemsAsync()
    {
        return await context.QuestItems.AsNoTracking().ToListAsync();
    }
}
