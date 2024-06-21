using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CPTWorkouts.Data.Migrations
{
    /// <inheritdoc />
    public partial class ControllersAndModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    ServicoFK = table.Column<int>(type: "int", nullable: false),
                    ClienteFK = table.Column<int>(type: "int", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => new { x.ClienteFK, x.ServicoFK });
                    table.ForeignKey(
                        name: "FK_Compras_Servicos_ServicoFK",
                        column: x => x.ServicoFK,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logotipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TreinadoresId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Carrinhos = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataCompra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipaFK = table.Column<int>(type: "int", nullable: true),
                    ServicosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Equipas_EquipaFK",
                        column: x => x.EquipaFK,
                        principalTable: "Equipas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Servicos_ServicosId",
                        column: x => x.ServicosId,
                        principalTable: "Servicos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_ServicoFK",
                table: "Compras",
                column: "ServicoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Equipas_TreinadoresId",
                table: "Equipas",
                column: "TreinadoresId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_EquipaFK",
                table: "Utilizadores",
                column: "EquipaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_ServicosId",
                table: "Utilizadores",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Utilizadores_ClienteFK",
                table: "Compras",
                column: "ClienteFK",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipas_Utilizadores_TreinadoresId",
                table: "Equipas",
                column: "TreinadoresId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Servicos_ServicosId",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipas_Utilizadores_TreinadoresId",
                table: "Equipas");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropTable(
                name: "Equipas");
        }
    }
}
