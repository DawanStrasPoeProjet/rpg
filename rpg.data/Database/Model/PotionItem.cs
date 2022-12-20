using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Data.Database.Model;

public class PotionItem
{
    [Key] public int PotionId { get; set; }
    public string Id { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public int Magic { get; set; }
    public int Price { get; set; }
}
