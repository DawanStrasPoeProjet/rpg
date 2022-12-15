using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public void Start()
        {
            Console.WriteLine("Le combat commence !");

            // On commence par déterminer qui commence en utilisant la méthode TurnOrder
            List<Entity.Entity> turnOrder = TurnOrder(Figher1, Fighter2);
            int turn = 0;
            
            while (Figher1.Health > 0 && Fighter2.Health > 0)
            {
                int maxTurn = turnOrder.Count;
                Console.WriteLine("------------------------------------------------");
                if (turnOrder[turn].isPlayer)
                    PlayerTurn(turnOrder[turn]);
                else
                    MonsterTurn(turnOrder[turn]);
                
                turn++;
                if (turn >= maxTurn)
                    turn = 0;
            }
        }

        // Interface de combat du joueur
        private void PlayerTurn(Entity.Entity player)
        {
            string? choice;
            do
            {
                Console.WriteLine($"{player.Name} : {player.Health}/{player.MaxHealth}\nArme : {player.Inventory.GetEquipedWeaponName()}");

                Console.WriteLine($@"Quelle sera votre action ?
                1 - Attaquer
                2 - Utiliser un objet
                3 - Passer son tour
                4 - Fuir le combat");

                Console.Write("Votre choix : ");
                choice = Console.ReadLine();
                if (choice is null)
                    choice = "0";
            } while (!CheckInput(choice));

            Console.WriteLine("Vous avez choisi : " + choice);

            switch (choice)
            {
                case "1":
                    Attack(player, player == Figher1 ? Fighter2 : Figher1);
                    break;
                case "2":
                    UseItem(player);
                    break;
                case "3":
                    break;
                case "4":
                    Flee();
                    break;
            }
        }

        // Interface de "réfléxion" de l'IA
        // Incomplète
        private void MonsterTurn(Entity.Entity monster)
        {
            
            Console.WriteLine("C'est au tour de " + monster.Name + " de jouer !");
            Attack(monster, monster == Figher1 ? Fighter2 : Figher1);
        }

        // Fonction principale de combat
        // Incomplète
        private void Attack(Entity.Entity attacker, Entity.Entity target)
        {
            Random rand = new();
            int attackAttempt = rand.Next(0, 21) + attacker.Attack;
            Debug.WriteLine($"{attacker.Name} atk({attacker.Attack}) : {attackAttempt} VS def : {target.Defense}");

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
                Console.WriteLine($"{attacker.Name} rate son attaque");
            }

        }

        //Fuit le combat
        private void Flee()
        {
            throw new NotImplementedException();
        }

        // Utilise un item dans l'inventaire
        private void UseItem(Entity.Entity figher1)
        {
            throw new NotImplementedException();
        }

   
        private List<Entity.Entity> TurnOrder(Entity.Entity fighter1, Entity.Entity fighter2)
        {
            Random random = new();
            List<Entity.Entity> turnOrder = new();
            int initiative1 = random.Next(1, 21) + fighter1.Initiative;
            int initiative2 = random.Next(1, 21) + fighter2.Initiative;

            if (initiative1 > initiative2)
            {
                turnOrder.Add(fighter1);
                turnOrder.Add(fighter2);
            }
            else
            {
                turnOrder.Add(fighter2);
                turnOrder.Add(fighter1);
            }

            return turnOrder;
        }
        

        // Vérifie que l'input est bien un chiffre entre 1 et 4
        private bool CheckInput(string input)
        {
            List<string> choices = new() { "1", "2", "3", "4" };
            if (choices.Contains(input))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Input incorrect");
                return false;
            }
        }
    }
}
