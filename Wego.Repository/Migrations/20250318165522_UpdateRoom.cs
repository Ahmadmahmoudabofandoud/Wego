using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomAddress",
                table: "Rooms",
                newName: "RoomLocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomLocation",
                table: "Rooms",
                newName: "RoomAddress");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
