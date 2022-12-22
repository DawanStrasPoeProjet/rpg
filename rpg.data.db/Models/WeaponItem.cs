#nullable disable

using System.ComponentModel.DataAnnotations;

namespace RPG.Data.Db.Models;

public class WeaponItem : Item
{
    [Key] public int WeaponItemId { get; set; }
    public int BaseDamage { get; set; }
    public int NumDiceRolls { get; set; }
    public int NumDiceFaces { get; set; }
}
