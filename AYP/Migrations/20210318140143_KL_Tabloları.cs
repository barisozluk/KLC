using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class KL_Tabloları : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KL_FizikselOrtam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KL_FizikselOrtam", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KL_GerilimTipi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KL_GerilimTipi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KL_Kapasite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KL_Kapasite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KL_KullanimAmaci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KL_KullanimAmaci", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "KL_FizikselOrtam",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Bakır" },
                    { 2, "Fiber Optik" }
                });

            migrationBuilder.InsertData(
                table: "KL_GerilimTipi",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "AC" },
                    { 2, "DC" }
                });

            migrationBuilder.InsertData(
                table: "KL_Kapasite",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Ethernet" },
                    { 2, "Fast Ethernet" },
                    { 3, "10-Gigabit Ethernet" },
                    { 4, "40-Gigabit Ethernet" }
                });

            migrationBuilder.InsertData(
                table: "KL_KullanimAmaci",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Girdi" },
                    { 2, "Çıktı" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KL_FizikselOrtam");

            migrationBuilder.DropTable(
                name: "KL_GerilimTipi");

            migrationBuilder.DropTable(
                name: "KL_Kapasite");

            migrationBuilder.DropTable(
                name: "KL_KullanimAmaci");
        }
    }
}
