using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferredDish_UserPreferences_UserPreferencesId",
                table: "PreferredDish");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferredCategories_Category_CategoryId",
                table: "UserPreferredCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferredCategories_UserPreferences_UserPreferencesId",
                table: "UserPreferredCategories");

            migrationBuilder.DropIndex(
                name: "IX_PreferredDish_UserPreferencesId",
                table: "PreferredDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferredCategories",
                table: "UserPreferredCategories");

            migrationBuilder.DropColumn(
                name: "UserPreferencesId",
                table: "PreferredDish");

            migrationBuilder.RenameTable(
                name: "UserPreferredCategories",
                newName: "UserPreferredCategory");

            migrationBuilder.RenameColumn(
                name: "UserPreferencesId",
                table: "UserPreferredCategory",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferredCategories_CategoryId",
                table: "UserPreferredCategory",
                newName: "IX_UserPreferredCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferredCategory",
                table: "UserPreferredCategory",
                columns: new[] { "UserId", "CategoryId" });

            migrationBuilder.CreateTable(
                name: "UserPreferredDishes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreferredDishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferredDishes", x => new { x.UserId, x.PreferredDishId });
                    table.ForeignKey(
                        name: "FK_UserPreferredDishes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferredDishes_PreferredDish_PreferredDishId",
                        column: x => x.PreferredDishId,
                        principalTable: "PreferredDish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferredDishes_PreferredDishId",
                table: "UserPreferredDishes",
                column: "PreferredDishId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferredCategory_AspNetUsers_UserId",
                table: "UserPreferredCategory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferredCategory_Category_CategoryId",
                table: "UserPreferredCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferredCategory_AspNetUsers_UserId",
                table: "UserPreferredCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferredCategory_Category_CategoryId",
                table: "UserPreferredCategory");

            migrationBuilder.DropTable(
                name: "UserPreferredDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPreferredCategory",
                table: "UserPreferredCategory");

            migrationBuilder.RenameTable(
                name: "UserPreferredCategory",
                newName: "UserPreferredCategories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserPreferredCategories",
                newName: "UserPreferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPreferredCategory_CategoryId",
                table: "UserPreferredCategories",
                newName: "IX_UserPreferredCategories_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "UserPreferencesId",
                table: "PreferredDish",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPreferredCategories",
                table: "UserPreferredCategories",
                columns: new[] { "UserPreferencesId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreferredDish_UserPreferencesId",
                table: "PreferredDish",
                column: "UserPreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredDish_UserPreferences_UserPreferencesId",
                table: "PreferredDish",
                column: "UserPreferencesId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferredCategories_Category_CategoryId",
                table: "UserPreferredCategories",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferredCategories_UserPreferences_UserPreferencesId",
                table: "UserPreferredCategories",
                column: "UserPreferencesId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
