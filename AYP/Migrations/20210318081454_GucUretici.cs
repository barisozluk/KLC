using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class GucUretici : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GucUretici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StokNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tanim = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiParcaNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Katalog = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Sembol = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    GucUreticiTurId = table.Column<int>(type: "int", nullable: false),
                    GirdiGucArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    CiktiGucArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    VerimlilikDegeri = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DahiliGucTuketimDegeri = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GucUretici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GucUretici_GucUreticiTur_GucUreticiTurId",
                        column: x => x.GucUreticiTurId,
                        principalTable: "GucUreticiTur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GucUretici_KL_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "KL_Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GucUretici_GucUreticiTurId",
                table: "GucUretici",
                column: "GucUreticiTurId");

            migrationBuilder.CreateIndex(
                name: "IX_GucUretici_TipId",
                table: "GucUretici",
                column: "TipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GucUretici");
        }
    }
}
