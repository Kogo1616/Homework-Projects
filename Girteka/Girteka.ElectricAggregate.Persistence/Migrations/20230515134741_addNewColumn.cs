using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Girteka.ElectricAggregate.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "FileLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FileLogs");
        }
    }
}
