﻿using RPG.Core;
using RPG.Data;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace RPG.Combat;

public class CombatSystem : ICombatSystem
{
    public IGame Game { get; set; } = null!;

    public CombatResult BeginCombat(IEntity source, IEnumerable<IEntity> entities)
    {
        Console.WriteLine("Le combat commence !\n");

        // On commence par déterminer qui commence en utilisant la méthode TurnOrder
        List<IEntity> turnOrder = TurnOrder(source, entities);
        
        int turn = 0;
        bool flee = false;

        while (!flee && // On ne peut pas s'enfuir si on est déjà en train de fuir
            turnOrder.Any(e => e.Health > 0) && // Il reste des combattants vivants
            turnOrder.Any(e => e.IsPlayer) // Il reste des joueurs vivants
        )
        {
            IEntity entity = turnOrder[turn];

            // Si l'entité est morte, on passe au tour suivant
            if (entity.Health <= 0)
            {
                turn = (turn + 1) % turnOrder.Count; //Reset à 0 si turn est égal à la taille de la liste
                continue;
            }

            // Si l'entité est un joueur, on demande à l'utilisateur ce qu'il veut faire
            if (entity.IsPlayer)
            {
                PlayerTurn(entity, entities);
            }
            // Sinon, on fait une action aléatoire
            else
            {
                // récupère player en comparant entities avec source
                MonsterTurn(entity, entities);
            }
        }
    }

    private int VictoryChecker(IEntity player, IEntity monster, bool flee)
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
    private bool PlayerTurn(IEntity player, IEnumerable<IEntity> entities)
    {
        int choice;
        do
        {

            Console.WriteLine($"{player.Name} : {player.Health}/{player.MaxHealth}\nArme : {player.EquippedItem.Name}");

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
                PlayerTurn(player, entities);
                break;
        }
        return false;
    }

    //Retourne une cible séléctionnée par l'utilisateur
    private IEntity ManualSelectTarget(IEntity source, IEnumerable<IEntity> entities)
    {
        List<IEntity> AliveTargets = new(entities.Where(e => e.Health > 0));
        IEntity target = source;

        while (target == source)
        {
            Console.WriteLine("Choisissez une cible :");
            for (int i = 0; i < entities.Count(); i++)
            {
                Console.WriteLine($"{i + 1} - {AliveTargets.ElementAt(i).Name}");
            }
            Console.Write("Votre choix : ");
            int choice = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            if (choice > 0 && choice <= entities.Count())
            {
                target = entities.ElementAt(choice - 1);
            }
            else
            {
                Console.WriteLine("Erreur : choix invalide");
            }
        }
        return target;
    }

    private IEntity AutoSelectTarget(IEntity source, IEnumerable<IEntity> entities, bool smartSelect = false)
    {
        List<IEntity> AliveTargets = new(entities.Where(e => e.Health > 0));
        Random rand = new();

        if (smartSelect)
        {
            int minHP = AliveTargets.Min(e => e.Health);
            int minDef = AliveTargets.Min(e => e.Defense);
            List<IEntity> minHPList = AliveTargets.Where(e => e.Health == minHP).ToList();
            List<IEntity> minDefList = AliveTargets.Where(e => e.Defense == minDef).ToList();

            // Choisit entre le moins de point de vie et le moins de défense aléatoirement, fait attention à ce qu'il y ait une cible valide
            if (minHPList.Count > 0 && minDefList.Count > 0)
            {
                return rand.Next(0, 2) == 0 ? minHPList.ElementAt(rand.Next(0, minHPList.Count)) : minDefList.ElementAt(rand.Next(0, minDefList.Count));
            }
            else if (minHPList.Count > 0)
            {
                return minHPList.ElementAt(rand.Next(0, minHPList.Count));
            }
            else if (minDefList.Count > 0)
            {
                return minDefList.ElementAt(rand.Next(0, minDefList.Count));
            }
            else
            {
                throw new Exception("Erreur : Aucune cible valide trouvée");
            }
        }
        else
        {
            return AliveTargets.ElementAt(rand.Next(0, AliveTargets.Count));
        }
    }

    // Interface de "réfléxion" de l'IA
    // Incomplète
    private void MonsterTurn(IEntity monster, IEntity player)
    {
        Console.WriteLine($"Tour de {monster.Name}.\n");
        Debug.WriteLineIf(monster.Health < monster.MaxHealth * 20 / 100, $"{monster.Name} doit se soigner");
        if (monster.Health < monster.MaxHealth * 20 / 100 && monster.Bag.Items.Count() > 0)
        {
            bool canHeal = AutoUseItem(monster, "IncreaseLifePotion");
            if (canHeal is false)
                Attack(monster, player);
        }
        else
        {
            Attack(monster, player);
        }
    }

    // Fonction principale de combat
    // Incomplète
    private void Attack(IEntity attacker, IEntity target)
    {
        Random rand = new();
        int attackAttempt = rand.Next(0, 21) + attacker.Attack;
        Console.WriteLine($"{attacker.Name} attaque {target.Name} avec {attacker.EquippedItem.Name} :\n");
        WeaponItem attackerWeapon = (WeaponItem)attacker.EquippedItem;

        if (attackAttempt > target.Defense)
        {
            int finalDamage = 0;
            for (int i = 0; i < attackerWeapon.NumDiceRolls; i++)
            {
                finalDamage += rand.Next(1, attackerWeapon.NumDiceFaces + 1);
            }
            finalDamage += attackerWeapon.BaseDamage;
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
    private bool Flee(IEntity coward, IEntity bully)
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
    private void UseItem(IEntity fighter, Item item)
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


    private bool AutoUseItem(IEntity fighter, string typeOfItem)
    {
        IEnumerable<Item> items = fighter.Inventory.Items;
        if (items.Count() == 0)
        {
            //Throw exception if inventory is empty
            //Exception thrown because it shouldn't be the case if AutoUseItem is used
            throw new Exception("Inventory empty");
        }

        foreach (Item iterableItem in items)
        {
            if (ItemsHelper.GetTagForId(iterableItem.Id) == "IncreaseLifePotion")
            {
                UseItem(fighter, iterableItem);
                return true;
            }
        }
        return false;
    }

    private void ManualUseItem(IEntity fighter)
    {
        int choice = 0;
        IEnumerable<IItem> items = fighter.Bag.Items;

        if (items.Count() == 0)
        {
            Console.WriteLine("Vous n'avez aucun objet dans votre inventaire");
            return;
        }

        int i = 1;
        foreach (IItem iterableItem in items)
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

    // Determine turn order depending of Initiative & Random
    private List<IEntity> TurnOrder(IEntity source, IEnumerable<IEntity> entities)
    {
        List<IEntity> entitiesList = entities.ToList();
        entitiesList.Add(source);

        List<int> rolls = new();
        for (int i = 0; i < entitiesList.Count() ; i++)
        {
            Random rand = new();
            rolls.Add(rand.Next(0, 21) + entitiesList[i].Initiative);
        }

        List<IEntity> orderedEntities = new();
        for (int i = 0; i < entitiesList.Count(); i++)
        {
            int max = rolls.Max();
            int index = rolls.IndexOf(max);
            orderedEntities.Add(entitiesList[index]);
            rolls.RemoveAt(index);
            entitiesList.RemoveAt(index);
        }
        
        Console.WriteLine("Lancement des dés d'initiative...");

        return orderedEntities;
    }


    // Vérifie que l'input est bien un chiffre entre 1 et 4
    // Sera supprimé si Spectre Console est implémenté
    private bool CheckInput(int input, int min = 1, int max = 4)
    {
        return input >= min && input <= max;
    }
}
