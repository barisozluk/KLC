using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class UcBirim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "UcBirimTur",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "GucUreticiTur",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "AgAnahtariTur",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "KL_Tip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KL_Tip", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UcBirim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StokNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tanım = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiParcaNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Katalog = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Sembol = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UcBirimTurId = table.Column<int>(type: "int", nullable: false),
                    GirdiAgArayuzuSayisi = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    CiktiAgArayuzuSayisi = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    GucArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UcBirim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UcBirim_KL_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "KL_Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UcBirim_UcBirimTur_UcBirimTurId",
                        column: x => x.UcBirimTurId,
                        principalTable: "UcBirimTur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KL_Tip",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Uç Birim" },
                    { 2, "Ağ Anahtarı" },
                    { 3, "Güç Üretici" },
                    { 4, "Uç Birim Ağ Arayüzü" },
                    { 5, "Ağ Anahtarı Ağ Arayüzü" },
                    { 6, "Uç Birim Güç Arayüzü" },
                    { 7, "Ağ Anahtarı Güç Arayüzü" },
                    { 8, "Güç Üretici Güç Arayüzü" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UcBirim_TipId",
                table: "UcBirim",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirim_UcBirimTurId",
                table: "UcBirim",
                column: "UcBirimTurId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UcBirim");

            migrationBuilder.DropTable(
                name: "KL_Tip");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "UcBirimTur",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "GucUreticiTur",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "AgAnahtariTur",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
