using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;
using RPG.Data.Database.Model;

namespace RPG.Data.Database.Dao;

public class EntityDAO : IEntityDAO
{
    private RpgContext context;

    public EntityDAO()
    {
        context = new RpgContext();
    }
        
    public List<Entity> GetEntities()
    {
        return context.Entities.AsNoTracking().ToList();
    }

    public Entity? FindEntityById(int id)
    {
        return context.Entities.Find(id);
    }

    void IEntityDAO.SaveOrUpdateEntity(Entity entity)
    {
        if (entity.EntityId == 0 )
        {
            context.Entities.Add(entity);
        }
        else
        {
            context.Entities.Update(entity);
        }
        context.SaveChanges();
    }

    void IEntityDAO.DeleteEntity(Entity entity)
    {

        context.Entities.Remove(entity);
        context.SaveChanges();
    }
}
