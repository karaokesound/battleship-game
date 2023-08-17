using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleshipGame.Data.Migrations
{
    public partial class gamemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameEntityId",
                table: "Fields",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameEntityId1",
                table: "Fields",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Player1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Player2Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Players_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Players_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Fields_GameEntityId",
                table: "Fields",
                column: "GameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_GameEntityId1",
                table: "Fields",
                column: "GameEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Id",
                table: "Games",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Id",
                table: "Games",
                column: "Player2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Games_GameEntityId",
                table: "Fields",
                column: "GameEntityId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Games_GameEntityId1",
                table: "Fields",
                column: "GameEntityId1",
                principalTable: "Games",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Games_GameEntityId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Games_GameEntityId1",
                table: "Fields");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Fields_GameEntityId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_GameEntityId1",
                table: "Fields");

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
                name: "GameEntityId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "GameEntityId1",
                table: "Fields");

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
