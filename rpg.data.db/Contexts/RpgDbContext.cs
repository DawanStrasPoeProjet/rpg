#nullable disable

using Microsoft.EntityFrameworkCore;

namespace RPG.Data.Db.Contexts;

public class RpgDbContext : DbContext
{
    private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RpgDb";

    public DbSet<Models.WeaponItem> WeaponItems { get; set; }
    public DbSet<Models.PotionItem> PotionItems { get; set; }
    public DbSet<Models.QuestItem> QuestItems { get; set; }
    public DbSet<Models.Entity> Entities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.WeaponItem>().HasData(
            new Models.WeaponItem
            {
                WeaponItemId = 1, Id = "fist", Tags = "fast", Name = "Poings", Description = "Des poings...",
                BaseDamage = 2, NumDiceRolls = 1, NumDiceFaces = 4
            },
            new Models.WeaponItem
            {
                WeaponItemId = 2, Id = "greatsword", Tags = "slow", Name = "Espadon", Description = "Un espadon...",
                BaseDamage = 6, NumDiceRolls = 1, NumDiceFaces = 12
            },
            new Models.WeaponItem
            {
                WeaponItemId = 3, Id = "sword", Tags = "fast", Name = "Épée", Description = "Une épée...",
                BaseDamage = 6, NumDiceRolls = 1, NumDiceFaces = 8
            },
            new Models.WeaponItem
            {
                WeaponItemId = 4, Id = "dagger", Tags = "fast", Name = "Dague", Description = "Une dague...",
                BaseDamage = 5, NumDiceRolls = 2, NumDiceFaces = 4
            },
            new Models.WeaponItem
            {
                WeaponItemId = 5, Id = "warhammer", Tags = "slow", Name = "Marteau de guerre",
                Description = "Un marteau de guerre...", BaseDamage = 6, NumDiceRolls = 1, NumDiceFaces = 8
            }
        );

        modelBuilder.Entity<Models.PotionItem>().HasData(
            new Models.PotionItem
            {
                PotionItemId = 1, Id = "small_life_potion", Tags = "healing,consumable", Name = "Potion de vie mineure",
                Description = "Une petite potion de vie mineure. Restaure 10 hp.", Health = 10
            },
            new Models.PotionItem
            {
                PotionItemId = 2, Id = "life_potion", Tags = "healing,consumable", Name = "Potion de vie",
                Description = "Une potion de vie. Restaure 20 hp.", Health = 20
            }
        );

        modelBuilder.Entity<Models.Entity>().HasData(
            new Models.Entity
            {
                EntityId = 1, Id = "player", Name = "Hero", Tags = "player",
                BagItemIds = "small_life_potion,life_potion,small_life_potion", DefaultEquippedItemId = "fist",
                EquippedItemId = "greatsword", Initiative = 12, Health = 80, MaxHealth = 80, Attack = 20, Defense = 15,
                Money = 0, IsPlayer = true
            },
            new Models.Entity
            {
                EntityId = 2, Id = "bandit", Name = "Bandit", DefaultEquippedItemId = "sword",
                BagItemIds = "small_life_potion", Health = 30, MaxHealth = 30, Initiative = 12, Attack = 15,
                Defense = 15
            },
            new Models.Entity
            {
                EntityId = 3, Id = "thief", Name = "Voleur", BagItemIds = "life_potion",
                DefaultEquippedItemId = "dagger", Health = 20, MaxHealth = 25, Initiative = 16, Attack = 17,
                Defense = 16
            },
            new Models.Entity
            {
                EntityId = 4, Id = "orc", Name = "Orc", DefaultEquippedItemId = "warhammer", Health = 40,
                MaxHealth = 40, Initiative = 8, Attack = 20, Defense = 10
            }
        );
    }
}
