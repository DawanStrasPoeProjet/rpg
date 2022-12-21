using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG.Data.Migrations
{
    public partial class AddAttackAndIsPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "Entities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlayer",
                table: "Entities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attack",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "IsPlayer",
                table: "Entities");
        }
    }
}
