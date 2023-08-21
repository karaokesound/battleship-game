using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleshipGame.Data.Migrations
{
    public partial class canshootplayerentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanShoot",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanShoot",
                table: "Players");
        }
    }
}
