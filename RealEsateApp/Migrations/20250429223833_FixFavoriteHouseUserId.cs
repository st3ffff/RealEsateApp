using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEsateApp.Migrations
{
    public partial class FixFavoriteHouseUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteHouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteHouses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteHouses_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Houses_AgentId",
                table: "Houses",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteHouses_HouseId",
                table: "FavoriteHouses",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteHouses_UserId",
                table: "FavoriteHouses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_AspNetUsers_AgentId",
                table: "Houses",
                column: "AgentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_AspNetUsers_AgentId",
                table: "Houses");

            migrationBuilder.DropTable(
                name: "FavoriteHouses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_AgentId",
                table: "Houses");
        }
    }
}
