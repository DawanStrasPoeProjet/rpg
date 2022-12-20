namespace RPG.Data.Database.Dao;

internal interface IEntityDAO
{
    Task<List<Model.Entity>> GetEntities();
    Task<Model.Entity> FindEntityById(int id);
    Task SaveOrUpdateEntity(Model.Entity entity);
    Task DeleteEntity(Model.Entity entity);
}
