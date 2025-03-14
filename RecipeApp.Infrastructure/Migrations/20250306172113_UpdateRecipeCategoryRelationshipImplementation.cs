using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeCategoryRelationshipImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeCategory",
                table: "RecipeCategory");

            migrationBuilder.DropIndex(
                name: "IX_RecipeCategory_RecipeId",
                table: "RecipeCategory");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Recipe",
                newName: "Recipe",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeCategory",
                table: "RecipeCategory",
                columns: new[] { "RecipeId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCategory_CategoryId",
                table: "RecipeCategory",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeCategory",
                table: "RecipeCategory");

            migrationBuilder.DropIndex(
                name: "IX_RecipeCategory_CategoryId",
                table: "RecipeCategory");

            migrationBuilder.RenameTable(
                name: "Recipe",
                schema: "dbo",
                newName: "Recipe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeCategory",
                table: "RecipeCategory",
                columns: new[] { "CategoryId", "RecipeId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCategory_RecipeId",
                table: "RecipeCategory",
                column: "RecipeId");
        }
    }
}
