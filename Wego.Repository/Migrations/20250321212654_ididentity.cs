using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wego.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ididentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId1",
                table: "HotelAmenities");

            migrationBuilder.DropIndex(
                name: "IX_HotelAmenities_HotelId1",
                table: "HotelAmenities");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "HotelAmenities");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "HotelAmenities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Amenities");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Amenities",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities",
                column: "Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "HotelAmenities");

            migrationBuilder.AddColumn<int>(
                name: "HotelId1",
                table: "HotelAmenities",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Amenities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Amenities");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Amenities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amenities",
                table: "Amenities",
                column: "Id");


            migrationBuilder.CreateIndex(
                name: "IX_HotelAmenities_HotelId1",
                table: "HotelAmenities",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelAmenities_Hotels_HotelId1",
                table: "HotelAmenities",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
