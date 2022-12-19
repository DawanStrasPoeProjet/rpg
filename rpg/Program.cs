using RPG.Combat;
using RPG.Core;
using RPG.Data;
using RPG.Inventory;

namespace RPG;

internal static class Program
{
    private static IGame CreateGame()
    {
        var itemSource = new YamlItemSource("items.yaml");
        var entitySource = new YamlEntitySource(itemSource, "entities.yaml");
        return new Game(itemSource, entitySource, new InventorySystem(), new CombatSystem());
    }

    public static void Main()
    {
        var ui = new UI.Class1();
        ui.Start();
    }
}
