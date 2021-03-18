using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class AgAnahtari : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgAnahtari",
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
                    AgAnahtariTurId = table.Column<int>(type: "int", nullable: false),
                    GirdiAgArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    CiktiAgArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    GucArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgAnahtari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgAnahtari_AgAnahtariTur_AgAnahtariTurId",
                        column: x => x.AgAnahtariTurId,
                        principalTable: "AgAnahtariTur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgAnahtari_KL_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "KL_Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtari_AgAnahtariTurId",
                table: "AgAnahtari",
                column: "AgAnahtariTurId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtari_TipId",
                table: "AgAnahtari",
                column: "TipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgAnahtari");
        }
    }
}
