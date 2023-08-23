using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleshipGame.Data.Migrations
{
    public partial class shipidsunkenships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SunkenShips",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShipId",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SunkenShips",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ShipId",
                table: "Fields");
        }
    }
}
