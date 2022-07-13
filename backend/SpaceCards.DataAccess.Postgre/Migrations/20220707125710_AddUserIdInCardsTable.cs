using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceCards.DataAccess.Postgre.Migrations
{
    public partial class AddUserIdInCardsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuessedCards_CardId",
                table: "GuessedCards");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Cards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GuessedCards_CardId",
                table: "GuessedCards",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuessedCards_CardId",
                table: "GuessedCards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Cards");

            migrationBuilder.CreateIndex(
                name: "IX_GuessedCards_CardId",
                table: "GuessedCards",
                column: "CardId",
                unique: true);
        }
    }
}
