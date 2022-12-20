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
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem(), new UISystem());
    }

    public static void Main()
    {
        var g = CreateGame();
    }
}
