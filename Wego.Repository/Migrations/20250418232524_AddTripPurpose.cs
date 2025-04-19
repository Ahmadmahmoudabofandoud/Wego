using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddTripPurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking");

            migrationBuilder.AlterColumn<int>(
                name: "RoomOptionId",
                table: "RoomBooking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialNeeds",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripPurpose",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking",
                column: "RoomOptionId",
                principalTable: "RoomOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialNeeds",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TripPurpose",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RoomOptionId",
                table: "RoomBooking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking",
                column: "RoomOptionId",
                principalTable: "RoomOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
