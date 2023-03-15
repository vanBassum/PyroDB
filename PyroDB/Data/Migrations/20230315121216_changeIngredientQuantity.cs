using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PyroDB.Migrations
{
    public partial class changeIngredientQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "Ingredients",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
