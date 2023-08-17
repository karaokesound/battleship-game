using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleshipGame.Data.Migrations
{
    public partial class addedmissinggetset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmpty",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHitted",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "City", "Name" },
                values: new object[] { 1, "City1", "Player1" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "City", "Name" },
                values: new object[] { 2, "City2", "Player2" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "City", "Name" },
                values: new object[] { 3, "City3", "Player3" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "City", "Name" },
                values: new object[] { 4, "City4", "Player4" });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "City", "Name" },
                values: new object[] { 5, "City5", "Player5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "IsEmpty",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IsHitted",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Fields");
        }
    }
}
