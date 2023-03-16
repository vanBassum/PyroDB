using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PyroDB.Migrations
{
    public partial class addRecipeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Recipes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Recipes",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Video",
                table: "Recipes");
        }
    }
}
