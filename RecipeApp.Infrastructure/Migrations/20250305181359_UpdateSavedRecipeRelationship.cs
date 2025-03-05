using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeApp.Infrastructure.Migrations
{
    public partial class UpdateSavedRecipeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old foreign key if it exists
            migrationBuilder.Sql(
                "IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Recipe_SavedRecipe_SavedRecipeUserId_SavedRecipeRecipeId') " +
                "ALTER TABLE Recipe DROP CONSTRAINT FK_Recipe_SavedRecipe_SavedRecipeUserId_SavedRecipeRecipeId;");

            // Drop the old index if it exists
            migrationBuilder.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Recipe_SavedRecipeUserId_SavedRecipeRecipeId' AND object_id = OBJECT_ID('Recipe')) " +
                "DROP INDEX IX_Recipe_SavedRecipeUserId_SavedRecipeRecipeId ON Recipe;");

            // Drop columns only if they exist
            migrationBuilder.Sql(
                "IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'SavedRecipeRecipeId' AND object_id = OBJECT_ID('Recipe')) " +
                "ALTER TABLE Recipe DROP COLUMN SavedRecipeRecipeId;");

            migrationBuilder.Sql(
                "IF EXISTS (SELECT 1 FROM sys.columns WHERE name = 'SavedRecipeUserId' AND object_id = OBJECT_ID('Recipe')) " +
                "ALTER TABLE Recipe DROP COLUMN SavedRecipeUserId;");

            // Drop and recreate the UserId foreign key to update cascade behavior
            migrationBuilder.DropForeignKey(
                name: "FK_SavedRecipe_AspNetUsers_UserId",
                table: "SavedRecipe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SavedAt",
                table: "SavedRecipe",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_SavedRecipe_RecipeId",
                table: "SavedRecipe",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedRecipe_AspNetUsers_UserId",
                table: "SavedRecipe",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedRecipe_Recipe_RecipeId",
                table: "SavedRecipe",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedRecipe_AspNetUsers_UserId",
                table: "SavedRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedRecipe_Recipe_RecipeId",
                table: "SavedRecipe");

            migrationBuilder.DropIndex(
                name: "IX_SavedRecipe_RecipeId",
                table: "SavedRecipe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SavedAt",
                table: "SavedRecipe",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

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

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe",
                columns: new[] { "SavedRecipeUserId", "SavedRecipeRecipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_SavedRecipe_SavedRecipeUserId_SavedRecipeRecipeId",
                table: "Recipe",
                columns: new[] { "SavedRecipeUserId", "SavedRecipeRecipeId" },
                principalTable: "SavedRecipe",
                principalColumns: new[] { "UserId", "RecipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SavedRecipe_AspNetUsers_UserId",
                table: "SavedRecipe",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}