using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batch_input_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "F2_BatchInput_History",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerminalID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Machine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start = table.Column<bool>(type: "bit", nullable: false),
                    Msnv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isComplate = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_F2_BatchInput_History", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "F2_BatchInput_ListPO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Po = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDGroup = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_F2_BatchInput_ListPO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_F2_BatchInput_ListPO_F2_BatchInput_History_IDGroup",
                        column: x => x.IDGroup,
                        principalTable: "F2_BatchInput_History",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_F2_BatchInput_ListPO_IDGroup",
                table: "F2_BatchInput_ListPO",
                column: "IDGroup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "F2_BatchInput_ListPO");

            migrationBuilder.DropTable(
                name: "F2_BatchInput_History");
        }
    }
}
