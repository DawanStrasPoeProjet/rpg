using Microsoft.EntityFrameworkCore;
using RpgAppDatabase.Model;

namespace RpgAppDatabase.Context
{
    public class RpgContext : DbContext
    {
        
        public DbSet<QuestItem> QuestItems { get; set; }
        public DbSet<WeaponItem> WeaponItems { get; set; }
        public DbSet<PotionItem> PotionItems { get; set; }
        public DbSet<Entity> Entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                    => optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RpgDatabase");
    
    }
}
