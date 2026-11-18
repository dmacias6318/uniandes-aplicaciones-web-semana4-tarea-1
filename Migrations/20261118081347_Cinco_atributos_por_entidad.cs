using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Provincias.Migrations
{
    /// <inheritdoc />
    public partial class Cinco_atributos_por_entidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Poblacion",
                table: "Provincias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CorreoElectronico",
                table: "Habitantes",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Poblacion",
                table: "Distritos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "CentrosSalud",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Cantones",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poblacion",
                table: "Provincias");

            migrationBuilder.DropColumn(
                name: "CorreoElectronico",
                table: "Habitantes");

            migrationBuilder.DropColumn(
                name: "Poblacion",
                table: "Distritos");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "CentrosSalud");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Cantones");
        }
    }
}
