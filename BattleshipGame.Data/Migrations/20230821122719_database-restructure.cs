using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleshipGame.Data.Migrations
{
    public partial class databaserestructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Games_GameEntityId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Games_GameEntityId1",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_Player1Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_Player2Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player1Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_Player2Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Fields_GameEntityId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_GameEntityId1",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "GameEntityId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "GameEntityId1",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "Player",
                table: "Fields");

            migrationBuilder.AddColumn<int>(
                name: "Game",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_PlayerId",
                table: "Fields",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Players_PlayerId",
                table: "Fields",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Players_PlayerId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_PlayerId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "Game",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Fields");

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

            migrationBuilder.AddColumn<string>(
                name: "Player",
                table: "Fields",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Id",
                table: "Games",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Id",
                table: "Games",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_GameEntityId",
                table: "Fields",
                column: "GameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_GameEntityId1",
                table: "Fields",
                column: "GameEntityId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_Player1Id",
                table: "Games",
                column: "Player1Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_Player2Id",
                table: "Games",
                column: "Player2Id",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
