using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Data.Migrations
{
    public partial class AddTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "WeaponItems",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "QuestItems",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "PotionItems",
                newName: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "WeaponItems",
                newName: "Tag");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "QuestItems",
                newName: "Tag");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "PotionItems",
                newName: "Tag");
        }
    }
}
