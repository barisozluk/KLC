using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class KapasiteEdit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 1,
                column: "Ad",
                value: "0-10 Ethernet");

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 2,
                column: "Ad",
                value: "10-100 Ethernet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 1,
                column: "Ad",
                value: "Ethernet");

            migrationBuilder.UpdateData(
                table: "KL_Kapasite",
                keyColumn: "Id",
                keyValue: 2,
                column: "Ad",
                value: "Fast Ethernet");
        }
    }
}
