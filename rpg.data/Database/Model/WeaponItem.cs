using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAppDatabase.Model
{
    public class WeaponItem
    {
        [Key]
        public int WeaponId { get; set; }
        public string Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int BaseDamage { get; set; }
        public int NumDiceRolls { get; set;}
        public int NumDiceFaces { get; set;}
                      
    }
}
