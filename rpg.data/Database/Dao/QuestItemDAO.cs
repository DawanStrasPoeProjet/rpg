using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class QuestItemDAO : IItemDAO<Model.QuestItem>
{
    private RpgContext context;

    //public QuestItemDAO(RpgContext context)
    //{
    //    this.context = context;
    //}
    public QuestItemDAO()
    {
        context= new RpgContext();
    }
    
    public List<Model.QuestItem> GetItems()
    {
        return context.QuestItems.AsNoTracking().ToList();
    }

    public Model.QuestItem? FindItemById(string id)
    {
        return context.QuestItems.AsNoTracking().SingleOrDefault(quest => quest.Id == id);        
    }

    public void SaveOrUpdateItem(Model.QuestItem item)
    {
        if (item.QuestId == 0)
        {
            context.QuestItems.Add(item);
        }
        else
        {
            context.Entry(item).State = EntityState.Modified;
        }
        context.SaveChanges();
    }

    public void DeleteItem(Model.QuestItem item)
    {
        context.QuestItems.Remove(item);
        context.SaveChanges();
    }
        
}
