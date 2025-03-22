using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpecification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId",
                table: "HotelAmenities");

            migrationBuilder.DropIndex(
                name: "IX_HotelAmenities_HotelId",
                table: "HotelAmenities");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "HotelAmenities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AmenityId",
                table: "HotelAmenities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId1",
                table: "HotelAmenities",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelAmenities",
                table: "HotelAmenities",
                columns: new[] { "HotelId", "AmenityId" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenities_HotelId1",
                table: "HotelAmenities",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId",
                table: "HotelAmenities",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId1",
                table: "HotelAmenities",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId",
                table: "HotelAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId1",
                table: "HotelAmenities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelAmenities",
                table: "HotelAmenities");

            migrationBuilder.DropIndex(
                name: "IX_HotelAmenities_HotelId1",
                table: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "HotelAmenities");

            migrationBuilder.AlterColumn<int>(
                name: "AmenityId",
                table: "HotelAmenities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "HotelAmenities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenities_HotelId",
                table: "HotelAmenities",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Amenities_AmenityId",
                table: "HotelAmenities",
                column: "AmenityId",
                principalTable: "Amenities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId",
                table: "HotelAmenities",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
