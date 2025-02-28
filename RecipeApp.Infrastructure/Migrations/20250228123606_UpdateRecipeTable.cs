using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Country_CountryId",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Recipe",
                newName: "PreferredDishId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_CountryId",
                table: "Recipe",
                newName: "IX_Recipe_PreferredDishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_PreferredDish_PreferredDishId",
                table: "Recipe",
                column: "PreferredDishId",
                principalTable: "PreferredDish",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_PreferredDish_PreferredDishId",
                table: "Recipe");

            migrationBuilder.RenameColumn(
                name: "PreferredDishId",
                table: "Recipe",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_PreferredDishId",
                table: "Recipe",
                newName: "IX_Recipe_CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Country_CountryId",
                table: "Recipe",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
