using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CPTWorkouts.Data.Migrations
{
    /// <inheritdoc />
    public partial class Clientes_Compras_Link : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCompra",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "ValorCompra",
                table: "Utilizadores");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCompra",
                table: "Utilizadores",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
