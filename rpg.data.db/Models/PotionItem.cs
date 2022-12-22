#nullable disable

using System.ComponentModel.DataAnnotations;

namespace RPG.Data.Db.Models;

public class PotionItem : Item
{
    [Key] public int PotionItemId { get; set; }
    public int Health { get; set; }
    public int Magic { get; set; }
}
