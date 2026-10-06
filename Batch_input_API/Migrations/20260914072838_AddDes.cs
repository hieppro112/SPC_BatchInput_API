using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batch_input_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isComplate",
                table: "F2_BatchInput_ListPO",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Po",
                table: "F2_BatchInput_ListPO",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionPO",
                table: "F2_BatchInput_ListPO",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionPO",
                table: "F2_BatchInput_ListPO");

            migrationBuilder.AlterColumn<bool>(
                name: "isComplate",
                table: "F2_BatchInput_ListPO",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Po",
                table: "F2_BatchInput_ListPO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
