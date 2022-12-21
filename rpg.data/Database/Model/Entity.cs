using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Data.Database.Model;

public class Entity
{
    [Key] public int EntityId { get; set; }
    public string Id { get; set; }
    public string Tags { get; set; } // "player,blah"
    public string BagItemIds { get; set; } // "small_life_potion,small_key"
    public string DefaultEquippedItemId { get; set; } // "fist"
    public string EquippedItemId { get; set; } 
    public string Name { get; set; }
    public int Initiative { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Magic { get; set; }
    public int MaxMagic { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Money { get; set; }
    public bool IsPlayer { get; set; }
}
