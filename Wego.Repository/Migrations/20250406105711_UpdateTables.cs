using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenityHotel_Hotels_HotelsId",
                table: "AmenityHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenityRoom_Rooms_RoomsId",
                table: "AmenityRoom");

            migrationBuilder.RenameColumn(
                name: "RoomsId",
                table: "AmenityRoom",
                newName: "AmenityRoomsId");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityRoom_RoomsId",
                table: "AmenityRoom",
                newName: "IX_AmenityRoom_AmenityRoomsId");

            migrationBuilder.RenameColumn(
                name: "HotelsId",
                table: "AmenityHotel",
                newName: "AmenityHotelsId");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityHotel_HotelsId",
                table: "AmenityHotel",
                newName: "IX_AmenityHotel_AmenityHotelsId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenityHotel_Hotels_AmenityHotelsId",
                table: "AmenityHotel",
                column: "AmenityHotelsId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenityRoom_Rooms_AmenityRoomsId",
                table: "AmenityRoom",
                column: "AmenityRoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AmenityHotel_Hotels_AmenityHotelsId",
                table: "AmenityHotel");

            migrationBuilder.DropForeignKey(
                name: "FK_AmenityRoom_Rooms_AmenityRoomsId",
                table: "AmenityRoom");

            migrationBuilder.RenameColumn(
                name: "AmenityRoomsId",
                table: "AmenityRoom",
                newName: "RoomsId");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityRoom_AmenityRoomsId",
                table: "AmenityRoom",
                newName: "IX_AmenityRoom_RoomsId");

            migrationBuilder.RenameColumn(
                name: "AmenityHotelsId",
                table: "AmenityHotel",
                newName: "HotelsId");

            migrationBuilder.RenameIndex(
                name: "IX_AmenityHotel_AmenityHotelsId",
                table: "AmenityHotel",
                newName: "IX_AmenityHotel_HotelsId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenityHotel_Hotels_HotelsId",
                table: "AmenityHotel",
                column: "HotelsId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AmenityRoom_Rooms_RoomsId",
                table: "AmenityRoom",
                column: "RoomsId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
