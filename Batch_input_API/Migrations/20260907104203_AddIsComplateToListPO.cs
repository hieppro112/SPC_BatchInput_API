using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batch_input_API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsComplateToListPO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isComplate",
                table: "F2_BatchInput_ListPO",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isComplate",
                table: "F2_BatchInput_ListPO");
        }
    }
}
