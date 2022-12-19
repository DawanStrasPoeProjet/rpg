using RPG.Core;
using Spectre.Console;

namespace RPG.UI;

public class Class1
{
    public void Start()
    {

        AnsiConsole.Write(CreateTable());
        // Ask the player
        var choix = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("What do you want [green]to do[/]?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
            .AddChoices(new[] { "Attaquer", "Esquiver", "Fuir", "Parler", "Changer d'arme", "Utiliser une potion", }));

        // Echo choix terminal
        AnsiConsole.WriteLine($"Vous avez {choix} !");
    }

    private Table CreateTable()
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


        return new Table()
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
    }
}
