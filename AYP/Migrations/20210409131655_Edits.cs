using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class Edits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adi",
                table: "GucArayuzu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "GucArayuzu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Adi",
                table: "AgArayuzu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "AgArayuzu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adi",
                table: "GucArayuzu");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "GucArayuzu");

            migrationBuilder.DropColumn(
                name: "Adi",
                table: "AgArayuzu");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "AgArayuzu");
        }
    }
}
