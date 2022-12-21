using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class PotionItemDAO : IItemDAO<Model.PotionItem>
{
    private RpgContext context;

    public PotionItemDAO()
    {
        context= new RpgContext();
    }
        

    public List<Model.PotionItem> GetItems()
    {
       return context.PotionItems.AsNoTracking().ToList();
    }

    public Model.PotionItem? FindItemById(string id)
    {
        return context.PotionItems.Find(id);
    }

    public void SaveOrUpdateItem(Model.PotionItem item)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(Model.PotionItem item)
    {
        if (item.PotionId == 0)
        {
            context.PotionItems.Add(item);
        }
        else
        {
            context.Entry(item).State= EntityState.Modified;
        }
        
        context.SaveChanges();
    }
}
