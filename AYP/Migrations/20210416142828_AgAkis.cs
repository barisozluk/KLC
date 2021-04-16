using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class AgAkis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxKapasite",
                table: "KL_Kapasite",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinKapasite",
                table: "KL_Kapasite",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 1,
                column: "MaxKapasite",
                value: 10);

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MaxKapasite", "MinKapasite" },
                values: new object[] { 100, 10 });

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MaxKapasite", "MinKapasite" },
                values: new object[] { 1000, 100 });

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "MaxKapasite", "MinKapasite" },
                values: new object[] { 10000, 1000 });

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "MaxKapasite", "MinKapasite" },
                values: new object[] { 40000, 10000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxKapasite",
                table: "KL_Kapasite");

            migrationBuilder.DropColumn(
                name: "MinKapasite",
                table: "KL_Kapasite");
        }
    }
}
