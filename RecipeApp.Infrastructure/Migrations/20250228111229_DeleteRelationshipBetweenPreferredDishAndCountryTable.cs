using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRelationshipBetweenPreferredDishAndCountryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferredDish_Country_CountryId",
                table: "PreferredDish");

            migrationBuilder.DropIndex(
                name: "IX_PreferredDish_CountryId",
                table: "PreferredDish");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "PreferredDish");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "PreferredDish",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PreferredDish_CountryId",
                table: "PreferredDish",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferredDish_Country_CountryId",
                table: "PreferredDish",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
