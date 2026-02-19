using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaginaparaSalvarVidas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosTipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalesEnAdopcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    FamiliaId = table.Column<int>(type: "int", nullable: false),
                    FechaAdopcion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalesEnAdopcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalesEnAdopcion_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalesEnAdopcion_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalesEnTransito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    FamiliaId = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalesEnTransito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalesEnTransito_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalesEnTransito_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalesPerdidosEncontrados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalesPerdidosEncontrados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalesPerdidosEncontrados_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnAdopcion_AnimalId",
                table: "AnimalesEnAdopcion",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnAdopcion_FamiliaId",
                table: "AnimalesEnAdopcion",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnTransito_AnimalId",
                table: "AnimalesEnTransito",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesEnTransito_FamiliaId",
                table: "AnimalesEnTransito",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalesPerdidosEncontrados_AnimalId",
                table: "AnimalesPerdidosEncontrados",
                column: "AnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalesEnAdopcion");

            migrationBuilder.DropTable(
                name: "AnimalesEnTransito");

            migrationBuilder.DropTable(
                name: "AnimalesPerdidosEncontrados");
        }
    }
}
