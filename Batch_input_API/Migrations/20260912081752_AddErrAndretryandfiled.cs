using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batch_input_API.Migrations
{
    /// <inheritdoc />
    public partial class AddErrAndretryandfiled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "F2_BatchInput_History",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isError",
                table: "F2_BatchInput_History",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "numRetry",
                table: "F2_BatchInput_History",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "F2_BatchInput_History");

            migrationBuilder.DropColumn(
                name: "isError",
                table: "F2_BatchInput_History");

            migrationBuilder.DropColumn(
                name: "numRetry",
                table: "F2_BatchInput_History");
        }
    }
}
