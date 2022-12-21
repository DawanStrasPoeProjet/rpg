using RPG.Combat;
using RPG.Core;
using RPG.Data;
using RPG.Inventory;
using RPG.UI;

namespace RPG;

internal static class Program
{
    private static IGame CreateGame()
    {
        var itemSource = new YamlItemSource("resources/items.yaml");
        var entitySource = new YamlEntitySource(itemSource, "resources/entities.yaml");
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem());
    }

    public static void Main()
    {
        //var g = CreateGame();

        //var player = g.EntitySource.Create("player");
        //var enemy1 = g.EntitySource.Create("swordsman1");
        //var enemy2 = g.EntitySource.Create("swordsman2");
        //var enemy3 = g.EntitySource.Create("swordsman3");

        //var result = g.CombatSystem.BeginCombat(player, new List<IEntity> { enemy1, enemy2, enemy3}) ;

        //Console.WriteLine(result switch
        //{
        //    CombatResult.Won => "You won!",
        //    CombatResult.Lost => "You lost!",
        //    CombatResult.Fled => "You fled!",
        //    _ => throw new ArgumentOutOfRangeException()
        //});

        //Lancer Start de Class1
        var c = new UISystem();
        c.Start();
    }
}
