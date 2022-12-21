using RPG.Core;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RPG.UI;

public class UISystem
{

    public void Start()
    {
        //Set utf8
        

        MainUI();

    }

    private void Fight()
    {
        var first = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .AddColumn(new TableColumn("[u]HP[/]"))
            .AddColumn(new TableColumn("[u]AP[/]"))
            .AddColumn(new TableColumn("[u]Ect[/]"))
            .AddRow("500", "150", "...");


        var second = new Table()
           .Border(TableBorder.Rounded)
           .BorderColor(Color.Red)
           .AddColumn(new TableColumn("[u]HP[/]"))
           .AddColumn(new TableColumn("[u]AP[/]"))
           .AddColumn(new TableColumn("[u]Ect[/]"))
           .AddRow("500", "[red]150[/]", "...");


        var last = new Table()
           .Centered()
           .Border(TableBorder.DoubleEdge)
           .Title("[yellow]TABLE[/]")
           .Caption("[yellow]QUESTION[/]")
           .AddColumn(new TableColumn(new Panel("[u]DONNEES PLAYER[/]").BorderColor(Color.Green)).Footer("[u]DESCRIPTION[/]"))
           //.AddColumn(new TableColumn("").Footer("[u]CHOIX 1[/]"))
           //.AddColumn(new TableColumn("").Footer("[u]CHOIX 2[/]"))

           .AddColumn(new TableColumn(""))
           .AddColumn(new TableColumn(new Panel("[u]DONNEES ENNEMI[/]").BorderColor(Color.Red)).Footer("[u]CHOIX 4[/]"))
           .AddRow(first, new Panel(""), second);
        //.AddColumn(new TableColumn("").Footer("[u]CHOIX 5[/]"))

        AnsiConsole.Write(last);
        // Ask the player
        var choix = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want [green]to do[/]?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
            .AddChoices(new[] { "Attaquer", "Esquiver", "Fuir", "Parler", "Changer d'arme", "Utiliser une potion", }));

        // Echo choix terminal
        AnsiConsole.WriteLine($"Vous avez {choix} !");
    }

    private void MainUI()
    {
        var info_player = new Panel(GenerateInfoPlayer("Bob", 34, 50, "Epée"));
        info_player.Border = BoxBorder.None;
        info_player.Padding = new Padding(0, 0, 0, 0);
        info_player.Expand = false;
        info_player.Collapse();

        Table world_info = GenerateInfoWorld("3", "les Catacombes");

        Table inventoryDisplay = new Table()
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0))
            .AddRow("INVENTAIRE")
            .AddRow(new Rule().RuleStyle("orange1"))
            .AddRow(GenerateInventoryData(RandomItemList()))
            .Border(TableBorder.None)
            .HideHeaders()
            .Expand();



        var entangledTable = new Table()
            .Border(TableBorder.None)
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0).Width(30))
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0).Width(100))
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0).Width(30))
            .AddRow(info_player, world_info, inventoryDisplay).Centered()
            .HideHeaders()
            
            .Expand();

        var mainTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0).Centered())
            .AddRow(entangledTable)
            .AddRow("\nBop\n")
            .Centered()
            .HideHeaders()
            .Expand();

        

        AnsiConsole.Write(mainTable);
    }

    private List<string> RandomItemList()
    {
        //Crée une liste d'objet random en différent quantité

        List<string> result = new();

        result.Add("Potion de soin");
        result.Add("Petite clé");
        result.Add("Potion de soin");
        result.Add("Potion de soin");
        result.Add("Epée");
        result.Add("Potion de soin");
        

        return result;
    }

    private Markup GenerateInfoPlayer(string name, int hp, int hpMax, string weapon)
    {
        return new Markup($"[bold]{name}[/]\n" +
            $"[bold]HP :[/] [italic]{hp}[/]/{hpMax}\n" +
            $"[bold]Arme équipée :[/] {weapon}");
    }

    private Table GenerateInfoWorld(string level, string zone)
    {
        var topRule = new Rule($"[yellow]EXPLORATION[/]");
        var levelInfo = new Markup($"[yellow]Niveau[/] : {level}");
        var zoneInfo = new Markup($"[yellow]Zone[/] : {zone.ToUpper()}");
        var bottomRule = new Rule();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .AddColumn(new TableColumn("").Padding(0, 0, 0, 0))
            .AddRow(topRule)
            .AddEmptyRow()
            .AddRow(levelInfo)
            .AddRow(zoneInfo)
            .AddEmptyRow()
            .HideHeaders();


        table.Columns[0].Centered();

        return table;

        //return $"[green]{level}[/]\nZone : {zone}";
    }

    
    //Sera utilisé dans le futur
    private string GenerateInventoryData(List<IItem> inventory)
    {
        //Return a string with the name and quantity, by counting, of each items
        //Prevent duplicates

        string data = "";
        List<string> items = new List<string>();
        
        foreach (IItem item in inventory)
        {
            if (!items.Contains(item.Name))
            {
                items.Add(item.Name);
                data += $"x{inventory.Count(x => x.Name == item.Name)} {item.Name}\n";
            }
        }

        return data;

    }


    //Surcharge pour permettre de faire les tests
    private string GenerateInventoryData(List<string> inventory)
    {
        //Return a string with the name and quantity, by counting, of each items
        //Prevent duplicates

        string data = "";
        List<string> items = new List<string>();

        foreach (string item in inventory)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
                data += $"x{inventory.Count(x => x == item)} {item}\n";
            }
        }

        return data;

    }
}
