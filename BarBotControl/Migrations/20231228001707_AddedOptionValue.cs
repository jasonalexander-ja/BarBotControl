using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarBotControl.Migrations
{
    public partial class AddedOptionValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OptionValue",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Options_OptionValue",
                table: "Options",
                column: "OptionValue",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Options_OptionValue",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "Options");
        }
    }
}
