using System.Diagnostics;
using RPG.Core;
using RPG.Data.Db.Contexts;
using RPG.Data.Db.Dao;

namespace RPG.Data.Db;

public class DbItemSource : IItemSource
{
    private int _key;

    private Models.Item? FindItem(string id)
    {
        using var ctx = new RpgDbContext();
        var weaponItemModel = new WeaponItemDao(ctx).FindItemById(id);
        if (weaponItemModel != null) return weaponItemModel;
        var potionItemModel = new PotionItemDao(ctx).FindItemById(id);
        if (potionItemModel != null) return potionItemModel;
        var questItemModel = new QuestItemDao(ctx).FindItemById(id);
        if (questItemModel != null) return questItemModel;
        return null;
    }

    public IItem Create(string id)
    {
        Debug.WriteLine($"DbItemSource::Create({id})");

        var itemModel = FindItem(id);

        if (itemModel is null)
            throw new Exception($"item with id={id} not found");

        itemModel.Tags ??= string.Empty;
        itemModel.Name ??= string.Empty;
        itemModel.Description ??= string.Empty;

        var tags = itemModel.Tags.Split(',');

        return itemModel switch
        {
            Models.WeaponItem weaponItemModel => new WeaponItem(++_key, id, weaponItemModel.Name,
                weaponItemModel.Description, tags, weaponItemModel.Price, weaponItemModel.BaseDamage,
                weaponItemModel.NumDiceRolls, weaponItemModel.NumDiceFaces),
            Models.PotionItem potionItemModel => new PotionItem(++_key, id, potionItemModel.Name,
                potionItemModel.Description, tags, potionItemModel.Price, potionItemModel.Health,
                potionItemModel.Magic),
            Models.QuestItem questItemModel => new QuestItem(++_key, id, questItemModel.Name,
                questItemModel.Description, tags, questItemModel.Price),
            _ => throw new Exception($"item with id={id} not found")
        };
    }

    public string GetName(string id)
    {
        var itemModel = FindItem(id);

        if (itemModel is null)
            throw new Exception($"item with id={id} not found");

        return itemModel switch
        {
            Models.WeaponItem weaponItemModel => weaponItemModel.Name ?? string.Empty,
            Models.PotionItem potionItemModel => potionItemModel.Name ?? string.Empty,
            Models.QuestItem questItemModel => questItemModel.Name ?? string.Empty,
            _ => throw new Exception($"item with id={id} not found")
        };
    }

    public string GetDescription(string id)
    {
        var itemModel = FindItem(id);

        if (itemModel is null)
            throw new Exception($"item with id={id} not found");

        return itemModel switch
        {
            Models.WeaponItem weaponItemModel => weaponItemModel.Description ?? string.Empty,
            Models.PotionItem potionItemModel => potionItemModel.Description ?? string.Empty,
            Models.QuestItem questItemModel => questItemModel.Description ?? string.Empty,
            _ => throw new Exception($"item with id={id} not found")
        };
    }
}
