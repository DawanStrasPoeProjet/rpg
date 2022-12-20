using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Data.Database.Model;

public class QuestItem
{
    [Key] public int QuestId { get; set; }
    public string Id { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
}
