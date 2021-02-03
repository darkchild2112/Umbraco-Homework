using Microsoft.EntityFrameworkCore.Migrations;

namespace Umbraco.Homework.API.Migrations
{
    public partial class addSerialNumberDataTypeToPrizeDraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "PrizeDrawEntries");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumberId",
                table: "PrizeDrawEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrizeDrawEntries_SerialNumberId",
                table: "PrizeDrawEntries",
                column: "SerialNumberId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrizeDrawEntries_SerialNumbers_SerialNumberId",
                table: "PrizeDrawEntries",
                column: "SerialNumberId",
                principalTable: "SerialNumbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrizeDrawEntries_SerialNumbers_SerialNumberId",
                table: "PrizeDrawEntries");

            migrationBuilder.DropIndex(
                name: "IX_PrizeDrawEntries_SerialNumberId",
                table: "PrizeDrawEntries");

            migrationBuilder.DropColumn(
                name: "SerialNumberId",
                table: "PrizeDrawEntries");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "PrizeDrawEntries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
