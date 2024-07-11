using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CPTWorkouts.Data.Migrations
{
    /// <inheritdoc />
    public partial class clienteoutreinador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Equipas_EquipaFK",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Servicos_ServicosId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_ServicosId",
                table: "Utilizadores");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "adm");

            migrationBuilder.DropColumn(
                name: "DataCompra",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "ServicosId",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "ValorCompra",
                table: "Utilizadores");

            migrationBuilder.AddColumn<string>(
                name: "TreinadorID",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
    name: "NumCliente",
    table: "Utilizadores",
    type: "int",
    nullable: true,
    defaultValue: 0);


            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Compras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCompra",
                table: "Compras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ValorCompraAux",
                table: "Compras",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ServicosTreinadores",
                columns: table => new
                {
                    ListaTreinadoresId = table.Column<int>(type: "int", nullable: false),
                    ServicosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosTreinadores", x => new { x.ListaTreinadoresId, x.ServicosId });
                    table.ForeignKey(
                        name: "FK_ServicosTreinadores_Servicos_ServicosId",
                        column: x => x.ServicosId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicosTreinadores_Utilizadores_ListaTreinadoresId",
                        column: x => x.ListaTreinadoresId,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_ServicosTreinadores_ServicosId",
                table: "ServicosTreinadores",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Equipas_EquipaFK",
                table: "Utilizadores",
                column: "EquipaFK",
                principalTable: "Equipas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Equipas_EquipaFK",
                table: "Utilizadores");

            migrationBuilder.DropTable(
                name: "ServicosTreinadores");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cl");

            migrationBuilder.DropColumn(
                name: "TreinadorID",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "ValorCompra",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "ValorCompraAux",
                table: "Compras");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCompra",
                table: "Utilizadores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicosId",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCompra",
                table: "Utilizadores",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "adm", null, "Administrativo", "ADMINISTRATIVO" });

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_ServicosId",
                table: "Utilizadores",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Equipas_EquipaFK",
                table: "Utilizadores",
                column: "EquipaFK",
                principalTable: "Equipas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Servicos_ServicosId",
                table: "Utilizadores",
                column: "ServicosId",
                principalTable: "Servicos",
                principalColumn: "Id");
        }
    }
}
