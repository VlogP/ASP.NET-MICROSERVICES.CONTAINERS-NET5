using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductMicroservice.DAL.Migrations
{
    public partial class UpdateProductAndClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "BsonId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BsonId_ClientId",
                table: "Products",
                columns: new[] { "BsonId", "ClientId" },
                unique: true,
                filter: "[BsonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ClientId",
                table: "Products",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Clients_ClientId",
                table: "Products",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Clients_ClientId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Products_BsonId_ClientId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ClientId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BsonId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
