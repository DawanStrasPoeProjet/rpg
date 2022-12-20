using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class PotionItemDAO : IItemDAO<Model.PotionItem>
{
    private RpgContext context;

    public PotionItemDAO(RpgContext context)
    {
        this.context = context;
    }

    public async Task SaveOrUpdateItemAsync(Model.PotionItem item)
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

    public async Task DeleteItemAsync(Model.PotionItem item)
    {
        context.PotionItems.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<Model.PotionItem> FindItemByIdAsync(int id)
    {
        return context.PotionItems.Find(id);
    }

    public async Task<List<Model.PotionItem>> GetItemsAsync()
    {
        return await context.PotionItems.AsNoTracking().ToListAsync();
    }
}
