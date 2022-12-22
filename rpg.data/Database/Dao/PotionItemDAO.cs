using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

public class PotionItemDAO : IItemDAO<Model.PotionItem>
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

    public Model.PotionItem? FindItemById(int potionId)
    {
        return context.PotionItems.Find(potionId);
    }

    public void SaveOrUpdateItem(Model.PotionItem item)
    {
        if (item.PotionId == 0)
        {
            context.PotionItems.Add(item);
        }
        else
        {
            context.Entry(item).State = EntityState.Modified;
        }

        context.SaveChanges();
    }

    public void DeleteItem(Model.PotionItem item)
    {
        context.PotionItems.Remove(item);        
        context.SaveChanges();
    }
}
