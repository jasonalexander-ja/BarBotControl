using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarBotControl.Migrations
{
    public partial class AddedErrorTypeValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ErrorValue",
                table: "ErrorTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorValue",
                table: "ErrorTypes");
        }
    }
}
