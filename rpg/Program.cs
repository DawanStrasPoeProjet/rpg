using RPG.Combat;
using RPG.Core;
using RPG.Data;
using RPG.Inventory;

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
        var g = CreateGame();
    }
}
