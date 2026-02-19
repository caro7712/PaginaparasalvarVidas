using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaparaSalvarVidas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosTablaComunitario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalesComunitarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    LugarHabitual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AptoParaAdopcion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalesComunitarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalesComunitarios_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesComunitarios_AnimalId",
                table: "AnimalesComunitarios",
                column: "AnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalesComunitarios");
        }
    }
}
