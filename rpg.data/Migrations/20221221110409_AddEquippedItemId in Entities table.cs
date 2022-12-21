using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Data.Migrations
{
    public partial class AddEquippedItemIdinEntitiestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquippedItemId",
                table: "Entities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquippedItemId",
                table: "Entities");
        }
    }
}
