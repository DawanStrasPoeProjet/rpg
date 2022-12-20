using Microsoft.EntityFrameworkCore;
using RPG.Data.Database.Context;

namespace RPG.Data.Database.Dao;

internal class EntityDAO : IEntityDAO
{
    private RpgContext context;

    public EntityDAO(RpgContext context)
    {
        this.context = context;
    }

    public async Task SaveOrUpdateEntity(Model.Entity entity)
    {
        if (entity.EntityId == 0)
        {
            context.Entities.Add(entity);
        }
        else
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteEntity(Model.Entity entity)
    {
        context.Entry(entity).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }

    public async Task<Model.Entity> FindEntityById(int id)
    {
        return await context.Entities.FindAsync(id);
    }

    public async Task<List<Model.Entity>> GetEntities()
    {
        return await context.Entities.AsNoTracking().ToListAsync();
    }
}
