using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarBotControl.Migrations
{
    public partial class AddedErrorTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorTypes",
                columns: table => new
                {
                    ErrorTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorMessage = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Recoverable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    SequenceId = table.Column<int>(type: "int", nullable: true, comment: "Recovery Sequence Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTypes", x => x.ErrorTypeId);
                    table.ForeignKey(
                        name: "FK_ErrorTypes_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErrorTypes_Sequences_SequenceId",
                        column: x => x.SequenceId,
                        principalTable: "Sequences",
                        principalColumn: "SequenceId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTypes_ModuleId",
                table: "ErrorTypes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTypes_SequenceId",
                table: "ErrorTypes",
                column: "SequenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorTypes");
        }
    }
}
