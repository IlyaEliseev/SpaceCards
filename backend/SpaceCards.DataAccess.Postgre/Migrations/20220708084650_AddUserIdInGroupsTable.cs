using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceCards.DataAccess.Postgre.Migrations
{
    public partial class AddUserIdInGroupsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");
        }
    }
}
