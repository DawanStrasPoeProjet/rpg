using RPG.Core;
using RPG.Data;
using System.Diagnostics;

namespace RPG.Combat;

public class CombatSystem : ICombatSystem
{
    public IGame Game { get; set; } = null!;

    private List<IEntity> AliveEntities { get; set; } = new List<IEntity>();
    private List<IEntity> AllEntites { get; set; } = new List<IEntity>();
    private List<IEntity> TurnOrder { get; set; } = new List<IEntity>();

    public CombatResult BeginCombat(IEntity source, IEnumerable<IEntity> entities)
    {
        Console.WriteLine("Le combat commence !\n");

        this.AliveEntities.Add(source);
        this.AliveEntities.AddRange(entities);

        //Afficher dans le débug la liste des entitées
        Debug.WriteLine("Liste des entités :");
        foreach (IEntity e in this.AliveEntities)
        {
            Debug.WriteLine($"{e.Name}");
        }

        this.AllEntites = this.AliveEntities;

        // On commence par déterminer qui commence en utilisant la méthode TurnOrder
        this.TurnOrder = GenerateTurnOrder();


        int turn = 0;
        bool flee = false;
        
        // Termine le combat si le joueur fuit // Il reste des combattants vivants // Il reste des joueurs vivants
        while (!flee && AliveEntities.Count > 1 && AliveEntities.Any(e => e.Equals(source)))
        {
            IEntity entity = TurnOrder[turn];

            // Si l'entité est morte, on passe au tour suivant
            if (entity.Health <= 0)
            {
                turn = (turn + 1) % TurnOrder.Count; //Reset à 0 si turn est égal à la taille de la liste
                continue;
            }
            
            if (entity.IsPlayer)
            {
                flee = PlayerTurn(entity);
            }
            else
            {
                // récupère player en comparant entities avec source
                MonsterTurn(entity, source);
            }

            // On passe au tour suivant
            turn = (turn + 1) % TurnOrder.Count;
        }

        // On détermine le résultat du combat
        if (flee && TurnOrder.Any(e => e.Equals(source) && e.Health > 0))
            return CombatResult.Fled;
        else if (TurnOrder.Any(e => e.Equals(source) && e.Health > 0))
            return CombatResult.Won;
        else
            return CombatResult.Lost;
    }

    // Interface de combat du joueur
    private bool PlayerTurn(IEntity player)
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
                IEntity target = ManualSelectTarget(player);
                Attack(player, target);
                
                break;
            case 2:
                ManualUseItem(player);
                break;
            case 3:
                break;
            case 4:
                return Flee(player) ;
            default:
                Console.WriteLine("Erreur : choix invalide");
                PlayerTurn(player);
                break;
        }
        return false;
    }

    // Interface de "réfléxion" de l'IA
    // Incomplète
    private void MonsterTurn(IEntity monster, IEntity source)
    {
        bool healed = false;

        Console.WriteLine($"Tour de {monster.Name}.\n");
        Debug.WriteLineIf(monster.Health < monster.MaxHealth * 20 / 100, $"{monster.Name} doit se soigner");


        if (monster.Health < monster.MaxHealth * 20 / 100 && monster.Bag.Items.Any())
            healed = AutoUseItem(monster, "IncreaseLifePotion");
        
        if (monster.Equals(source) && !healed)
        {
            Attack(monster, AutoSelectTarget(monster));
        }
        else if (!healed)
        {
            Attack(monster, source);
        }
    }

    //Retourne une cible séléctionnée par l'utilisateur
    private IEntity ManualSelectTarget(IEntity source)
    {
        List<IEntity> AliveTargets = AliveEntities.Where(e => !e.Equals(source)).ToList();
        
        IEntity target = source;

        while (target == source)
        {
            Console.WriteLine("Choisissez une cible :");
            for (int i = 0; i < AliveTargets.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {AliveTargets.ElementAt(i).Name}");
            }
            Console.Write("Votre choix : ");
            int choice = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            if (choice > 0 && choice <= AliveTargets.Count)
            {
                target = AliveTargets.ElementAt(choice - 1);
            }
            else
            {
                Console.WriteLine("Erreur : choix invalide");
            }
        }
        return target;
    }

    //Retourne une cible automatiquement avec ou sans recherche "intelligente"
    private IEntity AutoSelectTarget(IEntity source, bool smartSelect = false)
    {
        
        List<IEntity> AliveTargets = AliveEntities.Where(e => !e.Equals(source)).ToList();
        Random rand = new();

        if (!AliveTargets.Any())
            throw new Exception("Erreur : Aucune cible disponible");

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


    // Fonction principale de combat
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
            UpdateTargetStatus(target);
        }
        else
        {
            Console.WriteLine($"{attacker.Name} rate son attaque contre {target.Name}.");
        }
    }

    private void UpdateTargetStatus(IEntity target)
    {
        if (target.Health <= 0)
        {
            Console.WriteLine($"{target.Name} est mort !\n");
            
            target.Health = 0;
            AliveEntities.Remove(target);
        }
    }

    //Fuit le combat
    private bool Flee(IEntity coward)
    {
        Random rand = new();
        List<IEntity> bullies = AliveEntities.Where(e => !e.Equals(coward)).ToList();

        int fleeAttempt = rand.Next(1, 101) + (coward.Initiative - (bullies.Sum(e => e.Initiative) / bullies.Count));
        int fleeChance = bullies.Count * 20;
        
        if (fleeAttempt > fleeChance)
        {
            Console.WriteLine($"{coward.Name} a réussi à fuir le combat !");
            return true;
        }
        else
        {
            Console.WriteLine($"{coward.Name} n'a pas réussi à fuir le combat !");
            return false;
        }
    }

    //Utilise un item dans l'inventaire
    private static void UseItem(IEntity fighter, IItem item)
    {
        if (item is PotionItem potion)
        {
            fighter.Health += potion.Health;
            if (fighter.Health > fighter.MaxHealth)
                fighter.Health = fighter.MaxHealth;
            fighter.Bag.RemoveFirstItemById(potion.Id);
            Console.WriteLine($"{fighter.Name} utilise {potion.Name} et récupère {potion.Health} points de vie !");
            Console.WriteLine($"Santé de {fighter.Name} : {fighter.Health}/{fighter.MaxHealth}");
        }
        else
        {
            Console.WriteLine("Vous ne pouvez pas utiliser cet objet !");
        }
    }

    //Utilise automatiquement un item dans l'inventaire
    private static bool AutoUseItem(IEntity fighter, string typeOfItem)
    {
        IEnumerable<IItem> items = fighter.Bag.Items ;
        if (!items.Any())
        {
            //Throw exception if inventory is empty
            //Exception thrown because it shouldn't be the case if AutoUseItem is used
            throw new Exception("Inventory empty");
        }
        

        foreach (IItem iterableItem in items)
        {
            if (iterableItem.Tags.Contains(typeOfItem))
            {
                UseItem(fighter, iterableItem);
                return true;
            }
        }
        return false;
    }

    private static bool ManualUseItem(IEntity fighter)
    {
        IEnumerable<IItem> items = fighter.Bag.Items;

        if (!items.Any())
        {
            Console.WriteLine("Vous n'avez aucun objet dans votre inventaire");
            return false;
        }

        int i = 1;
        foreach (IItem iterableItem in items)
        {
            Console.WriteLine($"{i} - {iterableItem.Name}");
            i++;
        }

        Console.WriteLine($"{i} - Retour");

        int choice;
        do
        {
            Console.Write("Votre choix : ");

            choice = int.TryParse(Console.ReadLine(), out int itemResult) ? itemResult : 0;
        } while (!CheckInput(choice, max: i));

        if (choice == i)
        {
            return false;
        }
        else
        {
            UseItem(fighter, items.ElementAt(choice - 1));
            return true;
        }
    }

    // Determine turn order depending of Initiative & Random
    private List<IEntity> GenerateTurnOrder()
    {
        List<IEntity> entitiesList = this.AliveEntities.ToList();
        List<int> rolls = new();
        for (int i = 0; i < entitiesList.Count; i++)
        {
            Random rand = new();
            rolls.Add(rand.Next(1, 21) + entitiesList[i].Initiative);
        }

        List<IEntity> orderedEntities = new();
        while (entitiesList.Count > 0)
        {
            int maxRoll = rolls.Max();
            int maxRollIndex = rolls.IndexOf(maxRoll);
            orderedEntities.Add(entitiesList[maxRollIndex]);
            entitiesList.RemoveAt(maxRollIndex);
            rolls.RemoveAt(maxRollIndex);
        }

        Debug.WriteLine("Liste des entités dans l'ordre de tour :");
        foreach (IEntity e in orderedEntities)
        {
            Debug.WriteLine($"{e.Name} : {e.Initiative}");
        }

        Console.WriteLine("Lancement des dés d'initiative...");

        return orderedEntities;
    }


    // Vérifie que l'input est bien un chiffre entre 1 et 4
    // Sera supprimé si Spectre Console est implémenté
    private static bool CheckInput(int input, int min = 1, int max = 4)
    {
        return input >= min && input <= max;
    }
}
