using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class GucArayuzuEntitiesEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipId",
                table: "GucArayuzu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GucArayuzu_TipId",
                table: "GucArayuzu",
                column: "TipId");

            migrationBuilder.AddForeignKey(
                name: "FK_GucArayuzu_KL_Tip_TipId",
                table: "GucArayuzu",
                column: "TipId",
                principalTable: "KL_Tip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GucArayuzu_KL_Tip_TipId",
                table: "GucArayuzu");

            migrationBuilder.DropIndex(
                name: "IX_GucArayuzu_TipId",
                table: "GucArayuzu");

            migrationBuilder.DropColumn(
                name: "TipId",
                table: "GucArayuzu");
        }
    }
}
