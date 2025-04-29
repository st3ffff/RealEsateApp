using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEsateApp.Migrations
{
    public partial class AddTypeEnumToHouseTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForRent",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "RenterId",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Houses");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyType",
                table: "Houses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PropertyType",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsForRent",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RenterId",
                table: "Houses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
