using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceCards.DataAccess.Postgre.Migrations
{
    public partial class AddGroupIdIsNullToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Groups_GroupId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Cards",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Groups_GroupId",
                table: "Cards",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Groups_GroupId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Cards",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Groups_GroupId",
                table: "Cards",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
