using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Girteka.ElectricAggregate.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class testmigrati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Electricities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tinklas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pavadinimas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numeris = table.Column<int>(type: "int", nullable: true),
                    PPlus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PMinus = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electricities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Electricities");
        }
    }
}
