using System.Text;
using RPG.Core;
using Spectre.Console;

namespace RPG.Stage.UI;

public class UISystem : IUISystem
{
    private IEntity? _hero;
    private readonly Dictionary<string, object> _displayData = new();

    public UISystem()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void Invalidate()
    {
        AnsiConsole.Clear();

        var stageName = GetString("StageName");
        var sceneName = GetString("SceneName");

        if (string.IsNullOrWhiteSpace(sceneName))
            AnsiConsole.Write(new Rule($"{stageName}") { Justification = Justify.Left });
        else
            AnsiConsole.Write(new Rule($"{stageName}, {sceneName}"));
        AnsiConsole.WriteLine();

        if (_hero == null) return;
        AnsiConsole.MarkupLine($"[red underline]Vie        :[/] [red]{_hero.Health}/{_hero.MaxHealth}[/]");
        AnsiConsole.MarkupLine($"[green underline]Magie      :[/] [green]{_hero.Magic}/{_hero.MaxMagic}[/]");
        AnsiConsole.MarkupLine($"[yellow underline]Rubis      :[/] [yellow]{_hero.Money}[/]");
        var itemNames = _hero.Bag.Items.Select(x => x.Name);
        AnsiConsole.MarkupLine(
            $"[underline]Équipement :[/] [[{_hero.EquippedItem.Name}]] [[{string.Join(", ", itemNames)}]]");
    }

    public void SetHero(IEntity hero)
        => _hero = hero;

    private string GetString(string name)
        => _displayData.ContainsKey(name) ? _displayData[name].ToString()! : string.Empty;

    public void SetStageName(string name)
        => _displayData["StageName"] = name;

    public void SetStageDescription(string description)
        => _displayData["StageDescription"] = description;

    public void SetSceneName(string name)
        => _displayData["SceneName"] = name;

    public void SetSceneDescription(string description)
        => _displayData["SceneDescription"] = description;

    private void WaitEnterKeyPress()
    {
        Console.CursorVisible = false;
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
        Console.CursorVisible = true;
    }

    public void Say(IEntity from, string text)
    {
        Invalidate();
        Console.WriteLine();

        if (from.Name != string.Empty)
            AnsiConsole.MarkupLine($"[underline]{from.Name} :[/]");
        AnsiConsole.MarkupLine(text);

        AnsiConsole.MarkupLine("\n[rapidblink][[Enter]][/]");
        WaitEnterKeyPress();
    }

    public string Choices(string? title, IEnumerable<string> choices)
    {
        Invalidate();
        Console.WriteLine();

        var choicesList = choices.ToList();
        if (choicesList.Count == 1)
        {
            if (title != null)
            {
                AnsiConsole.MarkupLine(title);
                AnsiConsole.WriteLine();
            }

            AnsiConsole.MarkupLine($"[blue]> {choicesList.First()}[/]");
            WaitEnterKeyPress();

            return choicesList.First();
        }

        var selection = new SelectionPrompt<string>
        {
            WrapAround = true
        };
        selection.AddChoices(choicesList);

        if (title != null)
            selection.Title(title);

        return AnsiConsole.Prompt(selection);
    }
}
