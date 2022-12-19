using RPG.Core;
using RPG.Data;
using System.Diagnostics;

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
    private bool PlayerTurn(IEntity player)
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
    private void MonsterTurn(IEntity monster)
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
    private void Attack(IEntity attacker, IEntity target)
    {
        Random rand = new();
        int attackAttempt = rand.Next(0, 21) + attacker.Attack;
        Console.WriteLine($"{attacker.Name} attaque {target.Name} avec {attacker.EquippedItem.Name} :\n");

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
