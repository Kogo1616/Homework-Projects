using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Girteka.ElectricAggregate.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numeris",
                table: "Electricities");

            migrationBuilder.DropColumn(
                name: "Tipas",
                table: "Electricities");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.AddColumn<int>(
                name: "Numeris",
                table: "Electricities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipas",
                table: "Electricities",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
