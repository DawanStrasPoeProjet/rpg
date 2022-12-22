#nullable disable

using System.ComponentModel.DataAnnotations;

namespace RPG.Data.Db.Models;

public class QuestItem : Item
{
    [Key] public int QuestItemId { get; set; }
}
