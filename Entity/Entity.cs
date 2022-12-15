using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory;

namespace Projet_JDR.Entity
{
    internal class Entity
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Magic { get; set; }
        public int MaxMagic { get; set; }
        public Inventory.IBag Inventory { get; set; }
        public int Initiative { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public bool isPlayer { get; set; }

        public Entity(string name,
                      int health,
                      IBag inventory,
                      int initiative = 0,
                      int magic = 0,
                      int defense = 15,
                      int attack = 10,
                      int maxHealth = 0,
                      int maxMagic = 0,
                      bool player = false)
        {
            Name = name;
            Health = health > 0 ? health : throw new ArgumentOutOfRangeException(nameof(health));
            Inventory = inventory;
            Initiative = initiative < 0 ? 0 : initiative;
            Magic = magic < 0 ? 0 : magic;
            Attack = attack < 0 ? 0 : attack;
            Defense = defense < 0 ? 0 : defense;
            MaxHealth = maxHealth <= 0 ? health : maxHealth;
            MaxMagic = maxMagic < 0 ? magic : maxMagic;
            isPlayer = player;
        }
    }
}
