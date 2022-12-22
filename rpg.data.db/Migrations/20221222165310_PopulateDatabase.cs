using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Data.Db.Migrations
{
    public partial class PopulateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Entities",
                columns: new[] { "EntityId", "Attack", "BagItemIds", "DefaultEquippedItemId", "Defense", "EquippedItemId", "Health", "Id", "Initiative", "IsPlayer", "Magic", "MaxHealth", "MaxMagic", "Money", "Name", "Tags" },
                values: new object[,]
                {
                    { 1, 20, "small_life_potion,life_potion,small_life_potion", "fist", 15, "greatsword", 80, "player", 12, true, 0, 80, 0, 0, "Hero", "player" },
                    { 2, 15, "small_life_potion", "sword", 15, null, 30, "bandit", 12, false, 0, 30, 0, 0, "Bandit", null },
                    { 3, 17, "life_potion", "dagger", 16, null, 20, "thief", 16, false, 0, 25, 0, 0, "Voleur", null },
                    { 4, 20, null, "warhammer", 10, null, 40, "orc", 8, false, 0, 40, 0, 0, "Orc", null }
                });

            migrationBuilder.InsertData(
                table: "PotionItems",
                columns: new[] { "PotionItemId", "Description", "Health", "Id", "Magic", "Name", "Price", "Tags" },
                values: new object[,]
                {
                    { 1, "Une petite potion de vie mineure. Restaure 10 hp.", 10, "small_life_potion", 0, "Potion de vie mineure", 0, "healing,consumable" },
                    { 2, "Une potion de vie. Restaure 20 hp.", 20, "life_potion", 0, "Potion de vie", 0, "healing,consumable" }
                });

            migrationBuilder.InsertData(
                table: "WeaponItems",
                columns: new[] { "WeaponItemId", "BaseDamage", "Description", "Id", "Name", "NumDiceFaces", "NumDiceRolls", "Price", "Tags" },
                values: new object[,]
                {
                    { 1, 2, "Des poings...", "fist", "Poings", 4, 1, 0, "fast" },
                    { 2, 6, "Un espadon...", "greatsword", "Espadon", 12, 1, 0, "slow" },
                    { 3, 6, "Une épée...", "sword", "Épée", 8, 1, 0, "fast" },
                    { 4, 5, "Une dague...", "dagger", "Dague", 4, 2, 0, "fast" },
                    { 5, 6, "Un marteau de guerre...", "warhammer", "Marteau de guerre", 8, 1, 0, "slow" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Entities",
                keyColumn: "EntityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Entities",
                keyColumn: "EntityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Entities",
                keyColumn: "EntityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Entities",
                keyColumn: "EntityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PotionItems",
                keyColumn: "PotionItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PotionItems",
                keyColumn: "PotionItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeaponItems",
                keyColumn: "WeaponItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WeaponItems",
                keyColumn: "WeaponItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WeaponItems",
                keyColumn: "WeaponItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WeaponItems",
                keyColumn: "WeaponItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WeaponItems",
                keyColumn: "WeaponItemId",
                keyValue: 5);
        }
    }
}
