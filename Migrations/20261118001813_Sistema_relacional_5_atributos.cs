using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Provincias.Migrations
{
    /// <inheritdoc />
    public partial class Sistema_relacional_5_atributos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Habitantes");

            migrationBuilder.AddColumn<string>(
                name: "Capital",
                table: "Provincias",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capital",
                table: "Provincias");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Habitantes",
                type: "longtext",
                nullable: false);
        }
    }
}
