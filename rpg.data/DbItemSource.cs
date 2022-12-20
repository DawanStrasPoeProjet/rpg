using RPG.Core;

namespace RPG.Data;

public class DbItemSource : IItemSource
{
    private int _key;

    public IItem Create(string id)
    {
        using var ctx = new Database.Context.RpgContext();
        var daoPotions = new Database.Dao.PotionItemDAO(ctx);
        var potion = daoPotions.FindItemById(id);

        if (potion is null)
        {
            var daoWeapons = new Database.Dao.WeaponItemDAO(ctx);
            var weapon = daoPotions.FindItemById(id);

        }
        else
        {
            return new PotionItem(++_key, id, potion.Name, /* ... */);
        }
    }

    public string GetName(string id)
    {
        throw new NotImplementedException();
    }

    public string GetDescription(string id)
    {
        throw new NotImplementedException();
    }
}
