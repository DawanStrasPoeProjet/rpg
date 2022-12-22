using System.Text;
using RPG.Combat;
using RPG.Core;
using RPG.Data;
using RPG.Inventory;
using RPG.Stage;
using Spectre.Console;

namespace RPG;

internal static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var g = DoMenu(PromptHeroName());
        g?.StageSystem.Boot();
    }

    private static string PromptHeroName()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("RPG") { Justification = Justify.Left });
        return AnsiConsole.Ask<string>("Entrez le nom du héro :");
    }

    private static IGame? DoMenu(string heroName)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("RPG") { Justification = Justify.Left });
        AnsiConsole.WriteLine();

        var choices = new[] { "Histoire basée sur Ocarina of Time", "Combat de démo", "Quitter" };
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>
            {
                WrapAround = true
            }
            .AddChoices(choices));

        AnsiConsole.Clear();

        if (choice == choices[0])
            return CreateStoryScenario(heroName);
        if (choice == choices[1])
            return CreateCombatScenario(heroName);

        return null;
    }

    private static IGame CreateStoryScenario(string heroName)
    {
        var itemSource = new YamlItemSource("resources/oot/items.yaml");
        var entitySource = new YamlEntitySource(itemSource, "resources/oot/entities.yaml");
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem(),
            new StageSystem("resources/oot/intro.yaml", "start", heroName));
    }

    private static IGame CreateCombatScenario(string heroName)
    {
        var itemSource = new YamlItemSource("resources/items.yaml");
        var entitySource = new YamlEntitySource(itemSource, "resources/entities.yaml");
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem(),
            new DemoCombatStage(heroName));
    }
}
