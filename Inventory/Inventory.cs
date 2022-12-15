using Inventory;

namespace Inventory
{
    public enum ItemId
    {
        Fist,
        Sword,
        Axe,
        Pickaxe,
        SmallLifePotion,
        BigLifePotion,
        SmallMagicPotion,
        PoisonPotion,
        DoorKey,
        BossKey
    }

    public enum ItemCategory
    {
        None,
        Weapon,
        Potion,
        Quest
    }

    public class Item : ICloneable
    {
        public ItemId Id { get; set; }

        public virtual object Clone()
            => new Item { Id = Id };
    }

    public class WeaponItem : Item
    {
        public int Damage { get; set; }
        public int NumberOfDice { get; set; }
        public int DiceSize { get; set; }
        

        public override object Clone()
            => new WeaponItem { Id = Id, Damage = Damage };
    }

    public class PotionItem : Item
    {
        public int HealthPoints { get; set; }
        public int MagicPoints { get; set; }

        public override object Clone()
            => new PotionItem { Id = Id, HealthPoints = HealthPoints, MagicPoints = MagicPoints };
    }

    public class QuestItem : Item
    {
    }

    public static class ItemsHelper
    {
        private static readonly List<Item> Items = new()
        {
            new WeaponItem { Id = ItemId.Fist, Damage = 2, NumberOfDice = 1, DiceSize = 4 },
            new WeaponItem { Id = ItemId.Sword, Damage = 8, NumberOfDice = 1, DiceSize = 6 },
            new WeaponItem { Id = ItemId.Axe, Damage = 6, NumberOfDice = 1, DiceSize = 10 },
            new WeaponItem { Id = ItemId.Pickaxe, Damage = 10, NumberOfDice = 1, DiceSize = 4 },
            new PotionItem { Id = ItemId.SmallLifePotion, HealthPoints = 20 },
            new PotionItem { Id = ItemId.BigLifePotion, HealthPoints = 40 },
            new PotionItem { Id = ItemId.SmallMagicPotion, MagicPoints = 20 },
            new PotionItem { Id = ItemId.PoisonPotion, HealthPoints = -20 },
            new PotionItem { Id = ItemId.DoorKey },
            new PotionItem { Id = ItemId.BossKey }
        };

        private static readonly List<string> Names = new()
        {
            "Poings",
            "Épée",
            "Hache",
            "Pioche",
            "Petite potion de vie",
            "Grande potion de vie",
            "Petite potion de magie",
            "Potion de poison",
            "Clé de porte",
            "Clé de bosse",
        };

        private static readonly List<ItemCategory> Categories = new()
        {
            ItemCategory.Weapon,
            ItemCategory.Weapon,
            ItemCategory.Weapon,
            ItemCategory.Weapon,
            ItemCategory.Potion,
            ItemCategory.Potion,
            ItemCategory.Potion,
            ItemCategory.Potion,
            ItemCategory.Quest,
            ItemCategory.Quest
        };

        private static readonly List<string> Tags = new()
        {
            "FastWeapon",
            "FastWeapon",
            "SlowWeapon",
            "SlowWeapon",
            "IncreaseLifePotion",
            "IncreaseLifePotion",
            "IncreaseMagicPotion",
            "DecreaseLifePotion",
            "QuestKey",
            "QuestKey"
        };

        public static Item GetItemForId(ItemId id)
            => Items[(int)id];

        public static string GetNameForId(ItemId id)
            => Names[(int)id];

        public static ItemCategory GetCategoryForId(ItemId id)
            => Categories[(int)id];

        public static string GetTagForId(ItemId id)
            => Tags[(int)id];
    }

    public interface IBag
    {
        IEnumerable<Item> Items { get; }
        WeaponItem Weapon { get; }

        bool HasItemWithId(ItemId id);
        void AddItemById(ItemId id);
        bool RemoveFirstItemById(ItemId id);
        void RemoveAllItemsById(ItemId id);
        Item? GetFirstItemById(ItemId id);
        Item? TakeFirstItemById(ItemId id);
        bool HasItemWithTag(string tag);
        bool RemoveFirstItemByTag(string tag);
        void RemoveAllItemsByTag(string tag);
        Item? GetFirstItemByTag(string tag);
        Item? TakeFirstItemByTag(string tag);
        bool HasWeapon();
        string GetEquipedWeaponName();
        void RemoveWeapon();
        void EquipWeaponById(ItemId id);
        bool EquipWeapon(Item item);
        bool EquipFirstWeaponFromItemsById(ItemId id);
        bool EquipFirstWeaponFromItemsByTag(string tag);
        bool EquipFirstWeaponFromItems();
    }

    public class Bag : IBag
    {
        private List<Item> _items = new();
        public IEnumerable<Item> Items => _items;
        public WeaponItem Weapon { get; private set; } = (WeaponItem)ItemsHelper.GetItemForId(ItemId.Fist).Clone();

        public bool HasItemWithId(ItemId id)
            => GetFirstItemById(id) != null;

        public void AddItemById(ItemId id)
            => _items.Add((Item)ItemsHelper.GetItemForId(id).Clone());

        public bool RemoveFirstItemById(ItemId id)
        {
            var item = GetFirstItemById(id);
            if (item != null)
            {
                _items.Remove(item);
                return true;
            }
            return false;
        }

        public void RemoveAllItemsById(ItemId id)
            => _items.RemoveAll(x => x.Id == id);

        public Item? GetFirstItemById(ItemId id)
            => _items.Find(x => x.Id == id);

        public Item? TakeFirstItemById(ItemId id)
        {
            var item = GetFirstItemById(id);
            if (item != null)
                _items.Remove(item);
            return item;
        }

        public bool HasItemWithTag(string tag)
            => GetFirstItemByTag(tag) != null;

        public bool RemoveFirstItemByTag(string tag)
        {
            var item = GetFirstItemByTag(tag);
            if (item != null)
            {
                _items.Remove(item);
                return true;
            }
            return false;
        }

        public void RemoveAllItemsByTag(string tag)
            => _items.RemoveAll(x => ItemsHelper.GetTagForId(x.Id) == tag);

        public Item? GetFirstItemByTag(string tag)
            => _items.Find(x => ItemsHelper.GetTagForId(x.Id) == tag);

        public Item? TakeFirstItemByTag(string tag)
        {
            var item = GetFirstItemByTag(tag);
            if (item != null)
                _items.Remove(item);
            return item;
        }

        public bool HasWeapon()
            => Weapon != null;

        public string GetEquipedWeaponName()
            => ItemsHelper.GetNameForId(Weapon.Id);

        public void RemoveWeapon()
            => Weapon = (WeaponItem)ItemsHelper.GetItemForId(ItemId.Fist).Clone();

        public void EquipWeaponById(ItemId id)
            => Weapon = (WeaponItem)ItemsHelper.GetItemForId(id).Clone();

        public bool EquipWeapon(Item item)
        {
            Weapon = (WeaponItem)item;
            return item != null;
        }

        public bool EquipFirstWeaponFromItemsById(ItemId id)
        {
            var item = GetFirstItemById(id);
            if (item != null)
            {
                Weapon = (WeaponItem)item;
                _items.Remove(item);
                return true;
            }
            return false;
        }

        public bool EquipFirstWeaponFromItemsByTag(string tag)
        {
            var item = GetFirstItemByTag(tag);
            if (item != null)
            {
                Weapon = (WeaponItem)item;
                _items.Remove(item);
                return true;
            }
            return false;
        }

        public bool EquipFirstWeaponFromItems()
        {
            Item? item = null;
            foreach (var i in _items)
            {
                if (i is WeaponItem)
                {
                    item = i;
                    break;
                }
            }
            if (item != null)
            {
                Weapon = (WeaponItem)item;
                _items.Remove(item);
                return true;
            }
            return false;
        }
    }
}