using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarBotControl.Migrations
{
    public partial class FixedOptionIndexConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Options_OptionValue",
                table: "Options");

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionValue_ModuleId",
                table: "Options",
                columns: new[] { "OptionValue", "ModuleId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Options_OptionValue_ModuleId",
                table: "Options");

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionValue",
                table: "Options",
                column: "OptionValue",
                unique: true);
        }
    }
}
