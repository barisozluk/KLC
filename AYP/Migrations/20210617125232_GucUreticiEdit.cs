using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class GucUreticiEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GirdiGucArayuzuSayisi",
                table: "GucUretici",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GirdiGucArayuzuSayisi",
                table: "GucUretici",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
