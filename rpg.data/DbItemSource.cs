using RPG.Core;
using RPG.Data.Database.Dao;

namespace RPG.Data;

public class DbItemSource : IItemSource
{
    private int _key;
    private PotionItemDAO daoPotions = new Database.Dao.PotionItemDAO();
    private WeaponItemDAO daoWeapons = new Database.Dao.WeaponItemDAO();
    private QuestItemDAO daoQuests = new Database.Dao.QuestItemDAO();


    public IItem Create(string id)
    {        
        var potion = daoPotions.FindItemById(id);

        if (potion is null)
        {
            var weapon = daoWeapons.FindItemById(id);

            if (weapon is null)
            {
                var quest = daoQuests.FindItemById(id);
                return new QuestItem(++_key, id, quest.Name, quest.Description, quest.Tags.Split(','), quest.Price);
            }
            else
            {
                return new WeaponItem(++_key, id, weapon.Name, weapon.Description, weapon.Tags.Split(','),
                    weapon.Price, weapon.BaseDamage, weapon.NumDiceRolls, weapon.NumDiceFaces);
            }
        }
        else
        {
            return new PotionItem(++_key, id, potion.Name, potion.Description,
                potion.Tags.Split(','), potion.Price, potion.Health, potion.Magic);
        }

        throw new ArgumentException($"invalid item id `{id}`", nameof(id));
    }

    public string GetName(string id)
    {
        var potion = daoPotions.FindItemById(id);
        if (potion is null)
        {
            var weapon = daoWeapons.FindItemById(id);
            if (weapon is null)
            {
                var quest = daoQuests.FindItemById(id);
                return quest.Name;
            }
            else
            {
                return weapon.Name;
            }
        }
        else
        {
            return potion.Name;
        }
       
    }

    public string GetDescription(string id)
    {
        var potion = daoPotions.FindItemById(id);
        if (potion is null)
        {
            var weapon = daoWeapons.FindItemById(id);
            if (weapon is null)
            {
                var quest = daoQuests.FindItemById(id);
                return quest.Description;
            }
            else
            {
                return weapon.Description;
            }
        }
        else
        {
            return potion.Description;
        }            
    }
}
