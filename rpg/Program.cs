using RPG.Combat;
using RPG.Core;
using RPG.Inventory;

namespace RPG;

internal static class Program
{
    private static IGame CreateGame()
    {
        var itemSource = new DummyItemSource();
        var entitySource = new DummyEntitySource(itemSource);
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem());
    }

    public static void Main()
    {
        var g = CreateGame();
    }
}
