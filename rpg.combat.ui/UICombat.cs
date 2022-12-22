using RPG.Core;
using Spectre.Console;

namespace RPG.Combat.UI;

public class UICombat : IUICombat
{
    private IEntity Player { get; set; }
    private IEnumerable<IEntity> TurnOrder { get; set; }
    private IEnumerable<IEntity> AliveEntities { get; set; }
    private IEnumerable<IEntity> Enemies { get; set; }
    private string Description { get; set; }
    private int Turn { get; set; }

    public UICombat(IEntity player,
        IEnumerable<IEntity> turnOrder,
        IEnumerable<IEntity> aliveEntities,
        IEnumerable<IEntity> enemies,
        string description = "",
        int turn = -1)
    {
        this.Player = player;
        this.TurnOrder = turnOrder;
        this.AliveEntities = aliveEntities;
        this.Enemies = enemies;
        this.Description = description;
        this.Turn = turn;
    }

    public void Display()
    {
        Table player = PlayerTable(PlayerInfo());
        Table turn = TurnOrderTable(TurnOrderInfo());
        Table enemies = EnemiesTable(EnemiesInfo());

        AnsiConsole.Write(MainTable(EncapsulatedTable(player, turn, enemies), Description));
    }

    public void Update(IEntity? player = null,
        IEnumerable<IEntity>? turnOrder = null,
        IEnumerable<IEntity>? aliveEntities = null,
        IEnumerable<IEntity>? enemies = null,
        string? description = null,
        int turn = -1)
    {
        if (player is not null)
            this.Player = player;
        if (turnOrder is not null)
            this.TurnOrder = turnOrder;
        if (aliveEntities is not null)
            this.AliveEntities = aliveEntities;
        if (enemies is not null)
            this.Enemies = enemies;
        if (turn >= 0)
            this.Turn = turn;
        if (!string.IsNullOrEmpty(description))
            this.Description = description;

        Console.Clear();
        Display();
    }

    // ---- PROMPTS ----

    // Attend que l'utilisateur appuye sur la touche entrée
    public void WaitEnterKeyPress()
    {
        AnsiConsole.MarkupLine("\n[rapidblink][[Enter]][/]");
        Console.CursorVisible = false;
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
        Console.CursorVisible = true;
    }

    public dynamic? GetPrompt(PromptType prompt)
    {
        return prompt switch
        {
            PromptType.CombatMainSelection => ChoiceCombatMain(),
            PromptType.CombatTargetSelection => ChoiceCombatTarget(),
            PromptType.CombatItemSelection => ChoiceCombatItem(),
            _ => null,
        };
    }

    // Récupère le choix de l'interface principale de combat
    private static int ChoiceCombatMain()
    {
        int choice;

        string choix = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Que voulez-vous faire ?")
                .PageSize(4)
                .AddChoices(new string[]
                {
                    "1 - Attaquer",
                    "2 - Utiliser un objet",
                    "3 - Passer son tour",
                    "4 - Fuir le combat"
                }));
        //Récupérer le nombre dans choix et le mettre dans choice
        choice = int.Parse(choix.Split(" - ")[0]);
        return choice;
    }

    // Récupère la cible seléctionné par l'utilsateur
    private IEntity ChoiceCombatTarget()
    {
        IEntity target;
        IEnumerable<IEntity> AliveTargets = AliveEntities.Where(x => x != Player).ToList();

        List<string> possibleChoices = new();
        for (int i = 0; i < AliveTargets.Count(); i++)
        {
            possibleChoices.Add($"{i + 1} - {AliveTargets.ElementAt(i).Name}");
        }

        string choix = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Sur qui voulez-vous l'utiliser ?")
                .PageSize(10)
                .AddChoices(possibleChoices));

        int choice = int.Parse(choix.Split(" - ")[0]);
        target = AliveTargets.ElementAt(choice - 1);
        return target;
    }

    // Récupère l'item (ou pas) selectionné par l'utilisateur
    private IItem? ChoiceCombatItem()
    {
        IItem selectedItem;
        IEnumerable<IItem> items = Player.Bag.Items.Where(x => x.HasTags(Item.TagsOf("consumable")));

        List<string> possibleChoices = new();
        int i = 0;
        foreach (IItem item in items)
        {
            possibleChoices.Add($"{i + 1} - {item.Name}");
            i++;
        }
        possibleChoices.Add($"{i + 1} - Retour");

        string choix = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Quel objet voulez-vous utiliser ?")
                .PageSize(10)
                .AddChoices(possibleChoices));

        int choice = int.Parse(choix.Split(" - ")[0]);
        if (choice == i + 1)
            return null;
        selectedItem = items.ElementAt(choice - 1);
        return selectedItem;
    }


    // ---- TABLES ----

    // Table principale contenant la table encapsulée table et la description
    private static Table MainTable(Table encapsulated, string description)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn("").Centered())
            .AddRow(encapsulated)
            .AddRow(new Rule())
            .AddEmptyRow()
            .AddRow(description)
            .AddEmptyRow()
            .HideHeaders();

        return table;
    }

    // Table contenant les sous-tables du joueur, de l'ordre de tour et des ennemis
    private static Table EncapsulatedTable(Table player, Table turn, Table ennemy)
    {
        var table = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn(""))
            .AddColumn(new TableColumn(""))
            .AddColumn(new TableColumn(""))
            .AddRow(player, turn, ennemy)
            .HideHeaders();

        return table;
    }

    // Table contenant les informations du joueur
    private static Table PlayerTable(string playerInfo)
    {
        Table header = new Table()
            .AddColumn(new TableColumn(""))
            .AddEmptyRow()
            .AddRow("JOUEUR")
            .Centered()
            .HideHeaders()
            .Border(TableBorder.None);

        Rule separation = new Rule().RuleStyle("orange1");
        ;

        var table = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn(""))
            .AddRow(separation)
            .AddRow(playerInfo)
            .HideHeaders();

        table.InsertRow(0, header).Centered();
        return table;
    }

    // Table contenant les informations de l'ordre de tour
    private static Table TurnOrderTable(string turnOrder)
    {
        Table header = new Table()
            .AddColumn(new TableColumn(""))
            .AddRow("COMBAT")
            .Centered()
            .HideHeaders()
            .Border(TableBorder.None);

        Rule separation = new Rule().RuleStyle("orange1");
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn(new TableColumn(""))
            .AddRow(separation)
            .AddRow("Ordre de combat :")
            .AddEmptyRow()
            .AddRow(turnOrder)
            .Expand()
            .HideHeaders();

        table.InsertRow(0, header).Centered();
        return table;
    }

    // Table contenant les informations des ennemis
    private static Table EnemiesTable(string data)
    {
        Table header = new Table()
            .AddColumn(new TableColumn(""))
            .AddEmptyRow()
            .AddRow("ENNEMIS")
            .Centered()
            .HideHeaders()
            .Border(TableBorder.None);

        Rule separation = new Rule().RuleStyle("orange1");
        var table = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn(""))
            .AddRow(separation)
            .AddRow(data)
            .HideHeaders();

        table.InsertRow(0, header).Centered();
        return table;
    }


    // ---- STRING CREATION ----

    // Création du string contenant les informations du joueur
    private string PlayerInfo()
    {
        string name = $"Nom: {Player.Name} \n";
        string health = Player.Health > 0
            ? $"HP: {Player.Health}/{Player.MaxHealth} \n"
            : $"HP: [red]{Player.Health}/{Player.MaxHealth}[/]\n";
        string weapon = $"Arme: {Player.EquippedItem.Name} \n\n";
        string inventory = $"Inventaire:\n{PlayerInventoryInfo()}";

        return name + health + weapon + inventory;
    }

    // Création du string contenant les informations de l'inventaire du joueur
    private string PlayerInventoryInfo()
    {
        string data = "";
        List<string> items = new();

        foreach (IItem item in Player.Bag.Items.Where(e => e.Tags.Contains("consumable")))
        {
            if (!items.Contains(item.Name))
            {
                items.Add(item.Name);
                data += $"x{Player.Bag.Items.Count(x => x.Name == item.Name)} {item.Name}\n";
            }
        }
        return data;
    }

    // Création du string contenant l'ordre du tour
    private string TurnOrderInfo()
    {
        string data = "";
        for (int i = 0; i < TurnOrder.Count(); i++)
        {
            if (i == Turn)
                data += $"[green]> {TurnOrder.ElementAt(i).Name}[/] \n";
            else if (!AliveEntities.Contains(TurnOrder.ElementAt(i)))
                data += $"[red]  {TurnOrder.ElementAt(i).Name}[/]\n";
            else
                data += $"  {TurnOrder.ElementAt(i).Name} \n";
        }
        return data;
    }

    // Création du string contenant les informations des ennemis
    private string EnemiesInfo()
    {
        string name = "";
        string health = "";
        string weapon = "";
        string data = "";

        foreach (IEntity entity in Enemies)
        {
            name += $"Nom: {entity.Name} \n";

            if (AliveEntities.Contains(entity))
                health += $"HP: {entity.Health}/{entity.MaxHealth}\n";
            else
                health += $"HP: [red]0/{entity.MaxHealth}[/]\n";
            weapon += $"Arme: {entity.EquippedItem.Name} \n";
            data += name + health + weapon +
                    $"------------\n";

            name = "";
            health = "";
            weapon = "";
        }

        data = data.Remove(data.LastIndexOf("\n"));
        data = data.Remove(data.TrimEnd().LastIndexOf("\n"));

        return data;
    }
}
