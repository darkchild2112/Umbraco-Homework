using Microsoft.EntityFrameworkCore.Migrations;

namespace Umbraco.Homework.API.Migrations
{
    public partial class addUsesToSerialNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Uses",
                table: "SerialNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uses",
                table: "SerialNumbers");
        }
    }
}
