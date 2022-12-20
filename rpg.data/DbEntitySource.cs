using RPG.Core;

namespace RPG.Data;

public class DbEntitySource : IEntitySource
{
    private int _key;

    public IEntity Create(string id)
    {
        using var ctx = new Database.Context.RpgContext();
        var dao = new Database.Dao.EntityDAO(ctx);
        Database.Model.Entity entity = dao.FindItemById(id);
        return new Entity(_key, id, /* ... */);
    }
}
