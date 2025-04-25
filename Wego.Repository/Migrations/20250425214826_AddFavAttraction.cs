using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFavAttraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttractionId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AttractionId",
                table: "Favorites",
                column: "AttractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Attractions_AttractionId",
                table: "Favorites",
                column: "AttractionId",
                principalTable: "Attractions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Attractions_AttractionId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AttractionId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "AttractionId",
                table: "Favorites");
        }
    }
}
