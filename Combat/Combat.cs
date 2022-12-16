using Inventory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Projet_JDR.Combat
{
    internal class Combat
    {
        public Entity.Entity Figher1 { get; }
        public Entity.Entity Fighter2 { get; }
        
        public Combat(Entity.Entity figher1, Entity.Entity fighter2)
        {
            Figher1 = figher1 ?? throw new ArgumentNullException(nameof(figher1));
            Fighter2 = fighter2 ?? throw new ArgumentNullException(nameof(fighter2));
        }

        // Fonction principale du combat
        public int Start()
        {
            Console.WriteLine("Le combat commence !\n");

            // On commence par déterminer qui commence en utilisant la méthode TurnOrder
            List<Entity.Entity> turnOrder = TurnOrder(Figher1, Fighter2);
            int turn = 0;
            bool flee = false;
            
            while (Figher1.Health > 0 && Fighter2.Health > 0 && flee is false)
            {
                int maxTurn = turnOrder.Count;
                Console.WriteLine("------------------------------------------------");
                if (turnOrder[turn].isPlayer)
                    flee = PlayerTurn(turnOrder[turn]);
                else
                    MonsterTurn(turnOrder[turn]);
                
                turn++;
                if (turn >= maxTurn)
                    turn = 0;
            }
            return VictoryChecker(Figher1, Fighter2, flee);
        }

        private int VictoryChecker(Entity.Entity player, Entity.Entity monster, bool flee)
        {
            if (player.Health > 0 && monster.Health == 0)
            {
                Console.WriteLine($"Vous avez vaincu {monster.Name} !");
                return 0;
            }
            else if (player.Health == 0)
            {
                Console.WriteLine($"{monster.Name} vous a tué. Vous avez perdu.");
                return 1;
            }
            else if (flee)
            {
                Console.WriteLine("Vous avez fuit le combat.");
                return 2;
            }
            else
                throw new Exception($"Erreur inattendue dans la vérification de victoire!\n" +
                                    $"Player : {player.Health} / Monster : {monster.Health} / Flee : {flee}");
        }

        // Interface de combat du joueur
        private bool PlayerTurn(Entity.Entity player)
        {
            int choice;
            do
            {
                
                Console.WriteLine($"{player.Name} : {player.Health}/{player.MaxHealth}\nArme : {player.Inventory.GetEquipedWeaponName()}");

                Console.WriteLine($@"Quelle sera votre action ?
                1 - Attaquer
                2 - Utiliser un objet
                3 - Passer son tour
                4 - Fuir le combat");

                Console.Write("Votre choix : ");
                
                choice = int.TryParse(Console.ReadLine(), out int result) ? result : 0;

            } while (!CheckInput(choice));

            switch (choice)
            {
                case 1:
                    Attack(player, player == Figher1 ? Fighter2 : Figher1);
                    break;
                case 2:
                    ManualUseItem(player);
                    break;
                case 3:
                    break;
                case 4:
                    return Flee(player, player == Figher1 ? Fighter2 : Figher1);
                default:
                    Console.WriteLine("Erreur : choix invalide");
                    PlayerTurn(player);
                    break;
            }
            return false;
        }

        // Interface de "réfléxion" de l'IA
        // Incomplète
        private void MonsterTurn(Entity.Entity monster)
        {
            Console.WriteLine($"Tour de {monster.Name}.\n");
            Debug.WriteLineIf(monster.Health < monster.MaxHealth * 20 / 100, $"{monster.Name} doit se soigner");
            if (monster.Health < monster.MaxHealth * 20 / 100 && monster.Inventory.Items.Count() > 0)
            {
                bool canHeal = AutoUseItem(monster, "IncreaseLifePotion");
                if (canHeal is false)
                    Attack(monster, monster == Figher1 ? Fighter2 : Figher1);
            }
            else
            {
                Attack(monster, monster == Figher1 ? Fighter2 : Figher1);
            }
        }

        // Fonction principale de combat
        // Incomplète
        private void Attack(Entity.Entity attacker, Entity.Entity target)
        {
            Random rand = new();
            int attackAttempt = rand.Next(0, 21) + attacker.Attack;
            Console.WriteLine($"{attacker.Name} attaque {target.Name} avec {Inventory.ItemsHelper.GetNameForId(attacker.Inventory.Weapon.Id)} :\n");

            if (attackAttempt > target.Defense)
            {
                int finalDamage = 0;
                for (int i = 0; i < attacker.Inventory.Weapon.NumberOfDice; i++)
                {
                    finalDamage += rand.Next(1, attacker.Inventory.Weapon.DiceSize + 1);
                }
                finalDamage += attacker.Inventory.Weapon.Damage;
                target.Health -= finalDamage;
                if (target.Health < 0)
                    target.Health = 0;

                Console.WriteLine($"{attacker.Name} inflige {finalDamage} points de dégats à {target.Name} !\n");
                Console.WriteLine($"{target.Name} a maintenant {target.Health}/{target.MaxHealth} points de vie\n");
            }
            else
            {
                Console.WriteLine($"{attacker.Name} rate son attaque contre {target.Name}.");
            }
        }

        //Fuit le combat
        private bool Flee(Entity.Entity coward, Entity.Entity bully)
        {
            Random rand = new();

            int fleeAttempt = rand.Next(0, 21) + coward.Initiative - bully.Initiative;
            Debug.WriteLine($"{fleeAttempt} ({fleeAttempt - coward.Initiative + bully.Initiative} + {coward.Initiative} - {bully.Initiative}) VS {bully.Defense}");
            Debug.WriteLineIf(fleeAttempt > bully.Defense, "Le fuyard a réussi à fuir");
            if (fleeAttempt > bully.Defense)
            {
                Console.WriteLine($"{coward.Name} a réussi à fuir le combat !\n");
                return true;
            }
            else
            {
                Console.WriteLine($"{coward.Name} s'est fait rattrapé par {bully.Name} et l'attaque !\n");
                Attack(bully, coward);
                return false;
            }
        }

        // Utilise un item dans l'inventaire
        private void UseItem(Entity.Entity fighter, Item item)
        {
            if (item is PotionItem potion)
            {
                fighter.Health += potion.HealthPoints;
                if (fighter.Health > fighter.MaxHealth)
                    fighter.Health = fighter.MaxHealth;
                fighter.Inventory.RemoveFirstItemById(potion.Id);
                Console.WriteLine($"{fighter.Name} utilise {ItemsHelper.GetNameForId(potion.Id)} et récupère {potion.HealthPoints} points de vie !");
                Console.WriteLine($"Santé de {fighter.Name} : {fighter.Health}/{fighter.MaxHealth}");
            }
            else
            {
                Console.WriteLine("Vous ne pouvez pas utiliser cet objet !");
            }
        }

   
        private bool AutoUseItem(Entity.Entity fighter, string typeOfItem)
        {
            IEnumerable<Item> items = fighter.Inventory.Items;
            if(items.Count() == 0)
            {
                //Throw exception if inventory is empty
                //Exception thrown because it shouldn't be the case if AutoUseItem is used
                throw new Exception("Inventory empty");
            }

            foreach(Item iterableItem in items)
            {
                if(ItemsHelper.GetTagForId(iterableItem.Id) == "IncreaseLifePotion")
                {
                    UseItem(fighter, iterableItem);
                    return true;
                }
            }
            return false;
        }

        private void ManualUseItem(Entity.Entity fighter)
        {
            int choice = 0;
            IEnumerable<Item> items = fighter.Inventory.Items;

            if (items.Count() == 0)
            {
                Console.WriteLine("Vous n'avez aucun objet dans votre inventaire");
                return;
            }

            int i = 1;
            foreach (Item iterableItem in items)
            {
                Console.WriteLine($"{i} - {ItemsHelper.GetNameForId(iterableItem.Id)}");
                i++;
            }

            Console.WriteLine($"{i} - Retour");

            do
            {
                Console.Write("Votre choix : ");

                choice = int.TryParse(Console.ReadLine(), out int itemResult) ? itemResult : 0;
            } while (!CheckInput(choice, max: i));

            if (choice == i)
                PlayerTurn(fighter);
            else
                UseItem(fighter, items.ElementAt(choice - 1));
        }
        
        private List<Entity.Entity> TurnOrder(Entity.Entity fighter1, Entity.Entity fighter2)
        {
            Random random = new();
            List<Entity.Entity> turnOrder = new();
            int initiative1 = random.Next(1, 21) + fighter1.Initiative;
            int initiative2 = random.Next(1, 21) + fighter2.Initiative;

            Console.WriteLine("Lancement des dés d'initiative...");
            Console.WriteLine($"{fighter1.Name} : {initiative1}");
            Console.WriteLine($"{fighter2.Name} : {initiative2}");

            if (initiative1 > initiative2)
            {
                turnOrder.Add(fighter1);
                turnOrder.Add(fighter2);
            }
            else if (initiative1 == initiative2)
            {
                int decisiveRoll = random.Next(1, 3);
                if (decisiveRoll == 1)
                {
                    turnOrder.Add(fighter1);
                    turnOrder.Add(fighter2);
                }
                else
                {
                                    turnOrder.Add(fighter2);
                turnOrder.Add(fighter1);
                }
            }
            else
            {
                turnOrder.Add(fighter2);
                turnOrder.Add(fighter1);
            }

            return turnOrder;
        }


        // Vérifie que l'input est bien un chiffre entre 1 et 4
        private bool CheckInput(int input, int min = 1, int max = 4)
        {
            return input >= min && input <= max;
        }
    }
}
