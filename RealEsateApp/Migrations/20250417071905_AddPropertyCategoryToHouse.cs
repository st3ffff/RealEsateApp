using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEsateApp.Migrations
{
    public partial class AddPropertyCategoryToHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Houses");
        }
    }
}
