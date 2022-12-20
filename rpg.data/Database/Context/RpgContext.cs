using Microsoft.EntityFrameworkCore;

namespace RPG.Data.Database.Context;

public class RpgContext : DbContext
{
    public DbSet<Model.QuestItem> QuestItems { get; set; }
    public DbSet<Model.WeaponItem> WeaponItems { get; set; }
    public DbSet<Model.PotionItem> PotionItems { get; set; }
    public DbSet<Model.Entity> Entities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RpgDatabase");
}
