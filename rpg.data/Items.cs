using RPG.Core;

namespace RPG.Data;

public class WeaponItem : Item
{
    public int BaseDamage { get; set; }
    public int NumDiceRolls { get; set; }
    public int NumDiceFaces { get; set; }

    public WeaponItem(int key, string id, string name, string description, IEnumerable<string>? tags = null,
        int price = 0, int baseDamage = 0, int numDiceRolls = 0, int numDiceFaces = 0)
        : base(key, id, name, description, tags, price)
    {
        InsertFrontTag("weapon");
        BaseDamage = baseDamage;
        NumDiceRolls = numDiceRolls;
        NumDiceFaces = numDiceFaces;
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        return $"WeaponItem({baseStr.Substring(5, baseStr.Length - 6)}" +
               $", {nameof(BaseDamage)}={BaseDamage}" +
               $", {nameof(NumDiceRolls)}={NumDiceRolls}" +
               $", {nameof(NumDiceFaces)}={NumDiceFaces})";
    }
}

public class PotionItem : Item
{
    public int Health { get; set; }
    public int Magic { get; set; }

    public PotionItem(int key, string id, string name, string description, IEnumerable<string>? tags = null,
        int price = 0, int health = 0, int magic = 0)
        : base(key, id, name, description, tags, price)
    {
        InsertFrontTag("potion");
        Health = health;
        Magic = magic;
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        return $"PotionItem({baseStr.Substring(5, baseStr.Length - 6)}" +
               $", {nameof(Health)}={Health}" +
               $", {nameof(Magic)}={Magic})";
    }
}

public class QuestItem : Item
{
    public QuestItem(int key, string id, string name, string description, IEnumerable<string>? tags = null,
        int price = 0)
        : base(key, id, name, description, tags, price)
    {
        InsertFrontTag("quest");
    }

    public override string ToString()
    {
        var baseStr = base.ToString();
        return $"QuestItem({baseStr.Substring(5, baseStr.Length - 6)})";
    }
}
