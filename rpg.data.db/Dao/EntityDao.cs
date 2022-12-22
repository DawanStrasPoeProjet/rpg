using Microsoft.EntityFrameworkCore;
using RPG.Data.Db.Contexts;

namespace RPG.Data.Db.Dao;

public class EntityDao : IEntityDao
{
    private readonly RpgDbContext _ctx;

    public EntityDao(RpgDbContext ctx)
        => _ctx = ctx;

    public List<Models.Entity> GetEntities()
        => _ctx.Entities.AsNoTracking().ToList();

    public Models.Entity? FindEntityById(string id)
        => _ctx.Entities.FirstOrDefault(x => x.Id == id);

    public void SaveOrUpdateEntity(Models.Entity entity)
    {
        if (entity.EntityId == 0)
            _ctx.Entities.Add(entity);
        else
            _ctx.Entities.Update(entity);
        _ctx.SaveChanges();
    }

    public void DeleteEntity(Models.Entity entity)
    {
        _ctx.Entities.Remove(entity);
        _ctx.SaveChanges();
    }
}
