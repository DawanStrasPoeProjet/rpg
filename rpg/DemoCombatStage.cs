using RPG.Core;
using Spectre.Console;

namespace RPG;

public class DemoCombatStage : IStageSystem
{
    public IGame Game { get; set; } = null!;
    private readonly string _heroName;

    public DemoCombatStage(string heroName)
    {
        _heroName = heroName;
    }

    public void Boot()
    {
        var player = Game.EntitySource.Create("player");
        player.Rename(_heroName);

        var entities = new List<IEntity>()
        {
            Game.EntitySource.Create("bandit"),
            Game.EntitySource.Create("thief"),
            Game.EntitySource.Create("orc")
        };

        PrintResult(Game.CombatSystem.BeginCombat(player, entities));
    }

    private void PrintResult(CombatResult result)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("RPG") { Justification = Justify.Left });
        var resultStr = new[] { "gagné", "perdu", "fui" };
        AnsiConsole.WriteLine($"Vous avez {resultStr[(int)result]} !");
        Console.ReadKey();
    }
}
