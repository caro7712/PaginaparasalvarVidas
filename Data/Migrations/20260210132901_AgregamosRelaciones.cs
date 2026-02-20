using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaparaSalvarVidas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimalesEnTransito_AnimalId",
                table: "AnimalesEnTransito");

            migrationBuilder.DropIndex(
                name: "IX_AnimalesEnAdopcion_AnimalId",
                table: "AnimalesEnAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_AnimalesComunitarios_AnimalId",
                table: "AnimalesComunitarios");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Animales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnTransito_AnimalId",
                table: "AnimalesEnTransito",
                column: "AnimalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnAdopcion_AnimalId",
                table: "AnimalesEnAdopcion",
                column: "AnimalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesComunitarios_AnimalId",
                table: "AnimalesComunitarios",
                column: "AnimalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimalesEnTransito_AnimalId",
                table: "AnimalesEnTransito");

            migrationBuilder.DropIndex(
                name: "IX_AnimalesEnAdopcion_AnimalId",
                table: "AnimalesEnAdopcion");

            migrationBuilder.DropIndex(
                name: "IX_AnimalesComunitarios_AnimalId",
                table: "AnimalesComunitarios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Animales");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnTransito_AnimalId",
                table: "AnimalesEnTransito",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnAdopcion_AnimalId",
                table: "AnimalesEnAdopcion",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesComunitarios_AnimalId",
                table: "AnimalesComunitarios",
                column: "AnimalId");
        }
    }
}
