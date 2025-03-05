using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnexpectedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the index
            migrationBuilder.DropIndex(
                name: "IX_Recipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe");

            // Drop the foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_SavedRecipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe");

            // Now drop the columns
            migrationBuilder.DropColumn(
                name: "SavedRecipeRecipeId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "SavedRecipeUserId",
                table: "Recipe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-add the columns
            migrationBuilder.AddColumn<int>(
                name: "SavedRecipeRecipeId",
                table: "Recipe",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SavedRecipeUserId",
                table: "Recipe",
                type: "int",
                nullable: true);

            // Re-add the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_SavedRecipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe",
                columns: new[] { "SavedRecipeUserId", "SavedRecipeRecipeId" },
                principalTable: "SavedRecipe",
                principalColumns: new[] { "UserId", "RecipeId" });  // Adjust principal columns if incorrect

            // Re-add the index
            migrationBuilder.CreateIndex(
                name: "IX_Recipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe",
                columns: new[] { "SavedRecipeUserId", "SavedRecipeRecipeId" });
        }
    }
}
