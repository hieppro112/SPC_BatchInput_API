using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batch_input_API.Migrations
{
    /// <inheritdoc />
    public partial class Add_runtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RUNTIME",
                table: "F2_BatchInput_History",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RUNTIME",
                table: "F2_BatchInput_History");
        }
    }
}
