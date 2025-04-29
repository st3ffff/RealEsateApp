using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEsateApp.Migrations
{
    public partial class AddPropertyTypeAndIsForRentToHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForRent",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForRent",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Houses");
        }
    }
}
