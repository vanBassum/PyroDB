using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace PyroDB.Migrations
{
    public partial class AddChemicalFormula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataSourceInfoId",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataSourceInfoId",
                table: "Chemicals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Formula",
                table: "Chemicals",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataSourceInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DataSource = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSourceInfo", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_DataSourceInfoId",
                table: "Recipes",
                column: "DataSourceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Chemicals_DataSourceInfoId",
                table: "Chemicals",
                column: "DataSourceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chemicals_DataSourceInfo_DataSourceInfoId",
                table: "Chemicals",
                column: "DataSourceInfoId",
                principalTable: "DataSourceInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_DataSourceInfo_DataSourceInfoId",
                table: "Recipes",
                column: "DataSourceInfoId",
                principalTable: "DataSourceInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chemicals_DataSourceInfo_DataSourceInfoId",
                table: "Chemicals");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_DataSourceInfo_DataSourceInfoId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "DataSourceInfo");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_DataSourceInfoId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Chemicals_DataSourceInfoId",
                table: "Chemicals");

            migrationBuilder.DropColumn(
                name: "DataSourceInfoId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DataSourceInfoId",
                table: "Chemicals");

            migrationBuilder.DropColumn(
                name: "Formula",
                table: "Chemicals");
        }
    }
}
