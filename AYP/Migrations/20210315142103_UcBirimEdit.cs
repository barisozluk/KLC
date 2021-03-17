using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class UcBirimEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tanım",
                table: "UcBirim",
                newName: "Tanim");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tanim",
                table: "UcBirim",
                newName: "Tanım");
        }
    }
}
