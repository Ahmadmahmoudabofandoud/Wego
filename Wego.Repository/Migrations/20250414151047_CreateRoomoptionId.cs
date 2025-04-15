using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoomoptionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomOptionId",
                table: "RoomBooking",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomBooking_RoomOptionId",
                table: "RoomBooking",
                column: "RoomOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking",
                column: "RoomOptionId",
                principalTable: "RoomOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBooking_RoomOptions_RoomOptionId",
                table: "RoomBooking");

            migrationBuilder.DropIndex(
                name: "IX_RoomBooking_RoomOptionId",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "RoomOptionId",
                table: "RoomBooking");
        }
    }
}
