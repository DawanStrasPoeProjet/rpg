namespace RPG.Data.Db.Dao;

public interface IEntityDao
{
    List<Models.Entity> GetEntities();
    Models.Entity? FindEntityById(string id);
    void SaveOrUpdateEntity(Models.Entity entity);
    void DeleteEntity(Models.Entity entity);
}
