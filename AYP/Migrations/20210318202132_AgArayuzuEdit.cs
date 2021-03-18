using Microsoft.EntityFrameworkCore.Migrations;

namespace AYP.Migrations
{
    public partial class AgArayuzuEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipId",
                table: "AgArayuzu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgArayuzu_TipId",
                table: "AgArayuzu",
                column: "TipId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgArayuzu_KL_Tip_TipId",
                table: "AgArayuzu",
                column: "TipId",
                principalTable: "KL_Tip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgArayuzu_KL_Tip_TipId",
                table: "AgArayuzu");

            migrationBuilder.DropIndex(
                name: "IX_AgArayuzu_TipId",
                table: "AgArayuzu");

            migrationBuilder.DropColumn(
                name: "TipId",
                table: "AgArayuzu");
        }
    }
}
