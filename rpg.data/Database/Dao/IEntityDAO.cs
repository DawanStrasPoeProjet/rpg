namespace RPG.Data.Database.Dao;

public interface IEntityDAO
{
    List<Model.Entity> GetEntities();
    Model.Entity? FindEntityById(string id);
    void SaveOrUpdateEntity(Model.Entity entity);
    void DeleteEntity(Model.Entity entity);
}
