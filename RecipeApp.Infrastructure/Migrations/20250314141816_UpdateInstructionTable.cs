using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstructionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Instruction",
                table: "Instruction");

            migrationBuilder.DropIndex(
                name: "IX_Instruction_RecipeId",
                table: "Instruction");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Instruction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instruction",
                table: "Instruction",
                columns: new[] { "RecipeId", "InstructionNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Instruction",
                table: "Instruction");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Instruction",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instruction",
                table: "Instruction",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Instruction_RecipeId",
                table: "Instruction",
                column: "RecipeId");
        }
    }
}
