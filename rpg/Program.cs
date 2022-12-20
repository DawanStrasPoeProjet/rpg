using RPG.Combat;
using RPG.Core;
using RPG.Data;
using RPG.Inventory;
using RPG.Stage;
using RPG.UI;

namespace RPG;

internal static class Program
{
    private static IGame CreateGame()
    {
        var itemSource = new DbItemSource();
        var entitySource = new DbEntitySource();
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem(), new UISystem(),
            new StageSystem());
    }

    public static void Main()
    {
        var g = CreateGame();

        var entity = g.EntitySource.Create("player");
        var potion = g.ItemSource.Create("small_life_potion");
    }
}
