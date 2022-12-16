// See https://aka.ms/new-console-template for more information
using Inventory;
using Projet_JDR.Combat;
using Projet_JDR.Entity;

Console.WriteLine("Hello, World!");

IBag player_bag = new Bag();
IBag enemy_bag = new Bag();

player_bag.AddItemById(ItemId.SmallLifePotion);
player_bag.AddItemById(ItemId.SmallLifePotion);
player_bag.AddItemById(ItemId.Sword);

enemy_bag.AddItemById(ItemId.SmallLifePotion);
enemy_bag.AddItemById(ItemId.BossKey);
enemy_bag.AddItemById(ItemId.Axe);

player_bag.EquipFirstWeaponFromItems();
enemy_bag.EquipFirstWeaponFromItems();

Entity player = new Entity(name: "Héro",
                           health: 50,
                           initiative: 10,
                           inventory: player_bag,
                           player: false); // True : Utilise les méthodes de combat du joueur / False : Mode automatique, considéré comme une "IA"
                           
Entity enemy = new Entity(name: "Orc",
                          health: 50,
                          initiative: 10,
                          inventory: enemy_bag);

Combat combat = new Combat(player, enemy);

combat.Start();


//IBag bag = new Bag();
//bag.AddItemById(ItemId.BigLifePotion);
//bag.AddItemById(ItemId.BossKey);
//bag.AddItemById(ItemId.PoisonPotion);
//bag.AddItemById(ItemId.SmallLifePotion);
//bag.AddItemById(ItemId.SmallLifePotion);

//// Récupère la première potion de soin (et la retire de l'inventaire)
//var popo = bag.TakeFirstItemByTag("IncreaseLifePotion");

//bag.AddItemById(ItemId.Axe);

//Console.WriteLine("On souhaite équiper une arme :");
//if (!bag.EquipFirstWeaponFromItems())
//    Console.WriteLine("Aucune arme à équiper !");
//else
//    Console.WriteLine($"{ItemsHelper.GetNameForId(bag.Weapon!.Id)} équipé !");

//bag.AddItemById(ItemId.DoorKey);

//Console.WriteLine("\nOn souhaite ouvrir la porte fermée à clé :");
//if (bag.HasItemWithId(ItemId.DoorKey))
//{
//    bag.RemoveFirstItemById(ItemId.DoorKey);
//    Console.WriteLine("La porte est ouverte !");
//}
//else
//    Console.WriteLine("Impossible d'ouvrir la porte : il faut une clé !");

//Console.WriteLine("\n=== Inventaire ===");
//Console.WriteLine($"Arme : {(bag.Weapon is null ? "Aucune" : ItemsHelper.GetNameForId(bag.Weapon.Id))}");
//Console.WriteLine("Objets :");
//foreach (var i in bag.Items)
//    Console.WriteLine($"{ItemsHelper.GetNameForId(i.Id)} - {ItemsHelper.GetCategoryForId(i.Id)}");