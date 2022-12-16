using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TECH.Migrations
{
    public partial class create_parentId_for_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "partentId",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_partentId",
                table: "categories",
                column: "partentId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_partentId",
                table: "categories",
                column: "partentId",
                principalTable: "categories",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_partentId",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_partentId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "partentId",
                table: "categories");
        }
    }
}
