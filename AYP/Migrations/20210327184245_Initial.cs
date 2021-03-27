using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgAnahtariTur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgAnahtariTur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GucUreticiTur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GucUreticiTur", x => x.Id);
                });

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
                name: "UcBirimTur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UcBirimTur", x => x.Id);
                });

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
                    KatalogDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sembol = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SembolDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AgArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullanimAmaciId = table.Column<int>(type: "int", nullable: false),
                    FizikselOrtamId = table.Column<int>(type: "int", nullable: false),
                    KapasiteId = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgArayuzu_KL_FizikselOrtam_FizikselOrtamId",
                        column: x => x.FizikselOrtamId,
                        principalTable: "KL_FizikselOrtam",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgArayuzu_KL_Kapasite_KapasiteId",
                        column: x => x.KapasiteId,
                        principalTable: "KL_Kapasite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgArayuzu_KL_KullanimAmaci_KullanimAmaciId",
                        column: x => x.KullanimAmaciId,
                        principalTable: "KL_KullanimAmaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgArayuzu_KL_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "KL_Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GucArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullanimAmaciId = table.Column<int>(type: "int", nullable: false),
                    GerilimTipiId = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: true),
                    GirdiDuraganGerilimDegeri1 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GirdiDuraganGerilimDegeri2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GirdiDuraganGerilimDegeri3 = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GirdiMinimumGerilimDegeri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GirdiMaksimumGerilimDegeri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GirdiTukettigiGucMiktari = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CiktiDuraganGerilimDegeri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CiktiUrettigiGucKapasitesi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GucArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GucArayuzu_KL_GerilimTipi_GerilimTipiId",
                        column: x => x.GerilimTipiId,
                        principalTable: "KL_GerilimTipi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GucArayuzu_KL_KullanimAmaci_KullanimAmaciId",
                        column: x => x.KullanimAmaciId,
                        principalTable: "KL_KullanimAmaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GucArayuzu_KL_Tip_TipId",
                        column: x => x.TipId,
                        principalTable: "KL_Tip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    KatalogDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sembol = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SembolDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "UcBirim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StokNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tanim = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UreticiParcaNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Katalog = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    KatalogDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sembol = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SembolDosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UcBirimTurId = table.Column<int>(type: "int", nullable: false),
                    GirdiAgArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
                    CiktiAgArayuzuSayisi = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AgAnahtariAgArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgArayuzuId = table.Column<int>(type: "int", nullable: false),
                    AgAnahtariId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgAnahtariAgArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgAnahtariAgArayuzu_AgAnahtari_AgAnahtariId",
                        column: x => x.AgAnahtariId,
                        principalTable: "AgAnahtari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgAnahtariAgArayuzu_AgArayuzu_AgArayuzuId",
                        column: x => x.AgArayuzuId,
                        principalTable: "AgArayuzu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgAnahtariGucArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgAnahtariId = table.Column<int>(type: "int", nullable: false),
                    GucArayuzuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgAnahtariGucArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgAnahtariGucArayuzu_AgAnahtari_AgAnahtariId",
                        column: x => x.AgAnahtariId,
                        principalTable: "AgAnahtari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgAnahtariGucArayuzu_GucArayuzu_GucArayuzuId",
                        column: x => x.GucArayuzuId,
                        principalTable: "GucArayuzu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GucUreticiGucArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GucUreticiId = table.Column<int>(type: "int", nullable: false),
                    GucArayuzuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GucUreticiGucArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GucUreticiGucArayuzu_GucArayuzu_GucArayuzuId",
                        column: x => x.GucArayuzuId,
                        principalTable: "GucArayuzu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GucUreticiGucArayuzu_GucUretici_GucUreticiId",
                        column: x => x.GucUreticiId,
                        principalTable: "GucUretici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UcBirimAgArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgArayuzuId = table.Column<int>(type: "int", nullable: false),
                    UcBirimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UcBirimAgArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UcBirimAgArayuzu_AgArayuzu_AgArayuzuId",
                        column: x => x.AgArayuzuId,
                        principalTable: "AgArayuzu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UcBirimAgArayuzu_UcBirim_UcBirimId",
                        column: x => x.UcBirimId,
                        principalTable: "UcBirim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UcBirimGucArayuzu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UcBirimId = table.Column<int>(type: "int", nullable: false),
                    GucArayuzuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UcBirimGucArayuzu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UcBirimGucArayuzu_GucArayuzu_GucArayuzuId",
                        column: x => x.GucArayuzuId,
                        principalTable: "GucArayuzu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UcBirimGucArayuzu_UcBirim_UcBirimId",
                        column: x => x.UcBirimId,
                        principalTable: "UcBirim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AgAnahtariTur",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Kenar" },
                    { 2, "Toplama" },
                    { 3, "Omurga" }
                });

            migrationBuilder.InsertData(
                table: "GucUreticiTur",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Şebeke" },
                    { 2, "Güç Kaynağı" },
                    { 3, "Kesintisiz Güç Kaynağı" }
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
                    { 4, "10-Gigabit Ethernet" },
                    { 3, "Gigabit Ethernet" },
                    { 5, "40-Gigabit Ethernet" },
                    { 1, "Ethernet" },
                    { 2, "Fast Ethernet" }
                });

            migrationBuilder.InsertData(
                table: "KL_KullanimAmaci",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 1, "Girdi" },
                    { 2, "Çıktı" }
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

            migrationBuilder.InsertData(
                table: "UcBirimTur",
                columns: new[] { "Id", "Ad" },
                values: new object[,]
                {
                    { 3, "Video Wall" },
                    { 1, "Kamera" },
                    { 2, "NVR" },
                    { 4, "Sunucu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtari_AgAnahtariTurId",
                table: "AgAnahtari",
                column: "AgAnahtariTurId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtari_TipId",
                table: "AgAnahtari",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtariAgArayuzu_AgAnahtariId",
                table: "AgAnahtariAgArayuzu",
                column: "AgAnahtariId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtariAgArayuzu_AgArayuzuId",
                table: "AgAnahtariAgArayuzu",
                column: "AgArayuzuId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtariGucArayuzu_AgAnahtariId",
                table: "AgAnahtariGucArayuzu",
                column: "AgAnahtariId");

            migrationBuilder.CreateIndex(
                name: "IX_AgAnahtariGucArayuzu_GucArayuzuId",
                table: "AgAnahtariGucArayuzu",
                column: "GucArayuzuId");

            migrationBuilder.CreateIndex(
                name: "IX_AgArayuzu_FizikselOrtamId",
                table: "AgArayuzu",
                column: "FizikselOrtamId");

            migrationBuilder.CreateIndex(
                name: "IX_AgArayuzu_KapasiteId",
                table: "AgArayuzu",
                column: "KapasiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AgArayuzu_KullanimAmaciId",
                table: "AgArayuzu",
                column: "KullanimAmaciId");

            migrationBuilder.CreateIndex(
                name: "IX_AgArayuzu_TipId",
                table: "AgArayuzu",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_GucArayuzu_GerilimTipiId",
                table: "GucArayuzu",
                column: "GerilimTipiId");

            migrationBuilder.CreateIndex(
                name: "IX_GucArayuzu_KullanimAmaciId",
                table: "GucArayuzu",
                column: "KullanimAmaciId");

            migrationBuilder.CreateIndex(
                name: "IX_GucArayuzu_TipId",
                table: "GucArayuzu",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_GucUretici_GucUreticiTurId",
                table: "GucUretici",
                column: "GucUreticiTurId");

            migrationBuilder.CreateIndex(
                name: "IX_GucUretici_TipId",
                table: "GucUretici",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_GucUreticiGucArayuzu_GucArayuzuId",
                table: "GucUreticiGucArayuzu",
                column: "GucArayuzuId");

            migrationBuilder.CreateIndex(
                name: "IX_GucUreticiGucArayuzu_GucUreticiId",
                table: "GucUreticiGucArayuzu",
                column: "GucUreticiId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirim_TipId",
                table: "UcBirim",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirim_UcBirimTurId",
                table: "UcBirim",
                column: "UcBirimTurId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirimAgArayuzu_AgArayuzuId",
                table: "UcBirimAgArayuzu",
                column: "AgArayuzuId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirimAgArayuzu_UcBirimId",
                table: "UcBirimAgArayuzu",
                column: "UcBirimId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirimGucArayuzu_GucArayuzuId",
                table: "UcBirimGucArayuzu",
                column: "GucArayuzuId");

            migrationBuilder.CreateIndex(
                name: "IX_UcBirimGucArayuzu_UcBirimId",
                table: "UcBirimGucArayuzu",
                column: "UcBirimId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgAnahtariAgArayuzu");

            migrationBuilder.DropTable(
                name: "AgAnahtariGucArayuzu");

            migrationBuilder.DropTable(
                name: "GucUreticiGucArayuzu");

            migrationBuilder.DropTable(
                name: "UcBirimAgArayuzu");

            migrationBuilder.DropTable(
                name: "UcBirimGucArayuzu");

            migrationBuilder.DropTable(
                name: "AgAnahtari");

            migrationBuilder.DropTable(
                name: "GucUretici");

            migrationBuilder.DropTable(
                name: "AgArayuzu");

            migrationBuilder.DropTable(
                name: "GucArayuzu");

            migrationBuilder.DropTable(
                name: "UcBirim");

            migrationBuilder.DropTable(
                name: "AgAnahtariTur");

            migrationBuilder.DropTable(
                name: "GucUreticiTur");

            migrationBuilder.DropTable(
                name: "KL_FizikselOrtam");

            migrationBuilder.DropTable(
                name: "KL_Kapasite");

            migrationBuilder.DropTable(
                name: "KL_GerilimTipi");

            migrationBuilder.DropTable(
                name: "KL_KullanimAmaci");

            migrationBuilder.DropTable(
                name: "KL_Tip");

            migrationBuilder.DropTable(
                name: "UcBirimTur");
        }
    }
}
