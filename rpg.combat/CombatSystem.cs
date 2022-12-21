using RPG.Core;
using RPG.Data;
using RPG.UI;
using System.Diagnostics;

namespace RPG.Combat;

public class CombatSystem : ICombatSystem
{
    public IGame Game { get; set; } = null!;
    private IUICombat CombatUI { get; set; } = null!;

    private List<IEntity> AliveEntities { get; set; } = new List<IEntity>();
    private IEntity Player { get; set; } = null!;
    private List<IEntity> TurnOrder { get; set; } = new List<IEntity>();
    private int Turn { get; set; } = 0;

    public CombatResult BeginCombat(IEntity source, IEnumerable<IEntity> entities)
    {
        Debug.WriteLine("Le combat commence !\n");

        this.Player = source;
        this.AliveEntities.Add(source);
        this.AliveEntities.AddRange(entities);

        //Afficher dans le débug la liste des entitées
        Debug.WriteLine("Liste des entités :");
        foreach (IEntity e in this.AliveEntities)
        {
            Debug.WriteLine($"{e.Name}");
        }

        // On commence par déterminer qui commence en utilisant la méthode TurnOrder
        this.TurnOrder = GenerateTurnOrder();

        // Affichage de l'interface de combat
        CombatUI = new UICombat(source,
            TurnOrder,
            AliveEntities,
            entities,
            "Le combat commence !",
            Turn);
        CombatUI.Display();
        CombatUI.WaitEnterKeyPress();


        bool flee = false;
        
        // Termine le combat si le joueur fuit // Il reste des combattants vivants // Il reste des joueurs vivants
        while (!flee && AliveEntities.Count > 1 && AliveEntities.Any(e => e.Equals(source)))
        {
            IEntity entity = TurnOrder[Turn];

            // Si l'entité est morte, on passe au tour suivant
            if (entity.Health <= 0)
            {
                Turn = (Turn + 1) % TurnOrder.Count; //Reset à 0 si turn est égal à la taille de la liste
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
            Turn = (Turn + 1) % TurnOrder.Count;
        }

        CombatUI.Update(
            turnOrder: TurnOrder,
            aliveEntities: AliveEntities,
            player: Player,
            description: "Le combat est terminé !");

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



        //do
        //{

        //    Debug.WriteLine($"{player.Name} : {player.Health}/{player.MaxHealth}\nArme : {player.EquippedItem.Name}");

        //    Debug.WriteLine($@"Quelle sera votre action ?
        //        1 - Attaquer
        //        2 - Utiliser un objet
        //        3 - Passer son tour
        //        4 - Fuir le combat");

        //    Debug.Write("Votre choix : ");

        //    //choice = int.TryParse(Debug.ReadLine(), out int result) ? result : 0;

        //} while (!CheckInput(choice));
        CombatUI.Update(aliveEntities: AliveEntities, turn: Turn);
        choice = CombatUI.GetPrompt(PromptType.CombatMainSelection);
        
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
                Debug.WriteLine("Erreur : choix invalide");
                PlayerTurn(player);
                break;
        }
        return false;
    }

    // Interface de "réfléxion" de l'IA
    private void MonsterTurn(IEntity monster, IEntity source)
    {
        bool healed = false;

        Debug.WriteLine($"Tour de {monster.Name}.\n");
        Debug.WriteLineIf(monster.Health < monster.MaxHealth * 20 / 100, $"{monster.Name} doit se soigner");


        if (monster.Health < monster.MaxHealth * 20 / 100 && monster.Bag.Items.Any())
            healed = AutoUseItem(monster, "healing");
        
        if (monster.Equals(source) && !healed)
        {
            Attack(monster, AutoSelectTarget(monster, smartSelect: true));
        }
        else if (!healed)
        {
            Attack(monster, source);
        }
    }

    //Retourne une cible séléctionnée par l'utilisateur
    private IEntity ManualSelectTarget(IEntity source)
    {
        IEntity? target = CombatUI.GetPrompt(PromptType.CombatTargetSelection);

        if(target is null)
            throw new Exception("Erreur : cible invalide");
        return target;
        
        //List<IEntity> AliveTargets = AliveEntities.Where(e => !e.Equals(source)).ToList();

        //IEntity target = source;

        //while (target == source)
        //{
        //    Debug.WriteLine("Choisissez une cible :");
        //    for (int i = 0; i < AliveTargets.Count; i++)
        //    {
        //        Debug.WriteLine($"{i + 1} - {AliveTargets.ElementAt(i).Name}");
        //    }
        //    Debug.Write("Votre choix : ");
        //    //int choice = int.TryParse(Debug.ReadLine(), out int result) ? result : 0;


        //    if (choice > 0 && choice <= AliveTargets.Count)
        //    {
        //        target = AliveTargets.ElementAt(choice - 1);
        //    }
        //    else
        //    {
        //        Debug.WriteLine("Erreur : choix invalide");
        //    }
        //}
        //return target;
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

            if (minHPList.Count == 1)
            {
                return minHPList.First();
            }            
            else if (minDefList.Count == 1)
            {
                return minDefList.First();
            }
            else if (minHPList.Count > 1)
            {
                return minHPList.ElementAt(rand.Next(0, minHPList.Count));
            }
            else if (minDefList.Count > 1)
            {
                return minDefList.ElementAt(rand.Next(0, minDefList.Count));
            }
            else
            {
                throw new Exception("Erreur : Aucune cible disponible");
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
        Debug.WriteLine($"{attacker.Name} attaque {target.Name} avec {attacker.EquippedItem.Name} :\n");
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
            
            string dmgText = $"{attacker.Name} attaque {target.Name} avec {attacker.EquippedItem.Name} et lui inflige {finalDamage} points de dégâts !";
            string newHP = $"{target.Name} a maintenant {target.Health}/{target.MaxHealth} points de vie...";
            string deathText = "";
            string finalDesc = "";
            if (target.Health <= 0)
            {
                deathText = $"[red]{target.Name} est mort[/] !";

                target.Health = 0;
                AliveEntities.Remove(target);
                finalDesc = $"{dmgText}\n{newHP}\n{deathText}";
            }
            else
                finalDesc = $"{dmgText}\n{newHP}";

            ;
            //finalDesc = finalDesc.Remove(finalDesc.TrimEnd().LastIndexOf("\n"));
            Debug.WriteLine(finalDesc);

            CombatUI.Update(
                player: Player,
                aliveEntities: AliveEntities,
                description: finalDesc,
                turn: Turn);
            CombatUI.WaitEnterKeyPress();


            // Debug.WriteLine($"{attacker.Name} inflige {finalDamage} points de dégats à {target.Name} !\n");
            // Debug.WriteLine($"{target.Name} a maintenant {target.Health}/{target.MaxHealth} points de vie\n");
            
        }
        else
        {
            Debug.WriteLine($"{attacker.Name} rate son attaque contre {target.Name}.");
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
            Debug.WriteLine($"{coward.Name} a réussi à fuir le combat !");
            return true;
        }
        else
        {
            Debug.WriteLine($"{coward.Name} n'a pas réussi à fuir le combat !");
            return false;
        }
    }

    //Utilise un item dans l'inventaire
    private void UseItem(IEntity fighter, IItem item)
    {
        if (item is PotionItem potion)
        {
            fighter.Health += potion.Health;
            if (fighter.Health > fighter.MaxHealth)
                fighter.Health = fighter.MaxHealth;
            fighter.Bag.RemoveFirstItemById(potion.Id);

            string usePotion = $"{fighter.Name} utilise {potion.Name} et récupère {potion.Health} points de vie !";
            string currentHealth = $"Santé de {fighter.Name} : {fighter.Health}/{fighter.MaxHealth}";
            
            Debug.WriteLine($"{fighter.Name} utilise {potion.Name} et récupère {potion.Health} points de vie !");
            Debug.WriteLine($"Santé de {fighter.Name} : {fighter.Health}/{fighter.MaxHealth}");

            CombatUI.Update(
                player: Player,
                aliveEntities: AliveEntities,
                description: $"{usePotion}\n{currentHealth}",
                turn: Turn);
            CombatUI.WaitEnterKeyPress();
        }
        else
        {
            Debug.WriteLine("Vous ne pouvez pas utiliser cet objet !");
        }
    }

    //Utilise automatiquement un item dans l'inventaire
    private bool AutoUseItem(IEntity fighter, string typeOfItem)
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

    private bool ManualUseItem(IEntity fighter)
    {
        //IEnumerable<IItem> items = fighter.Bag.Items;

        //if (!items.Any())
        //{
        //    Debug.WriteLine("Vous n'avez aucun objet dans votre inventaire");
        //    return false;
        //}

        //int i = 1;
        //foreach (IItem iterableItem in items)
        //{
        //    Debug.WriteLine($"{i} - {iterableItem.Name}");
        //    i++;
        //}

        //Debug.WriteLine($"{i} - Retour");

        //int choice;
        //do
        //{
        //    Debug.Write("Votre choix : ");

        //    //choice = int.TryParse(Debug.ReadLine(), out int itemResult) ? itemResult : 0;
        //} while (!CheckInput(choice = 0, max: i));

        IItem? consumableItem = CombatUI.GetPrompt(PromptType.CombatItemSelection);

        if (consumableItem is null)
            return false;
        else
        {
            UseItem(fighter, consumableItem);

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

        Debug.WriteLine("Lancement des dés d'initiative...");

        return orderedEntities;
    }


    // Vérifie que l'input est bien un chiffre entre 1 et 4
    // Sera supprimé si Spectre Console est implémenté
    //private static bool CheckInput(int input, int min = 1, int max = 4)
    //{
    //    return input >= min && input <= max;
    //}
}
