using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CPTWorkouts.Data.Migrations
{
    public partial class TroubleShoot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ServicosId",
                table: "Utilizadores");

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

            // Delete existing role if it exists
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE [Id] = 'cl'");

            // Insert the new role
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cl", null, "Cliente", "CLIENTE" });

            migrationBuilder.CreateIndex(
                name: "IX_ServicosTreinadores_ServicosId",
                table: "ServicosTreinadores",
                column: "ServicosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicosTreinadores");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cl");

            migrationBuilder.AddColumn<int>(
                name: "ServicosId",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_ServicosId",
                table: "Utilizadores",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Servicos_ServicosId",
                table: "Utilizadores",
                column: "ServicosId",
                principalTable: "Servicos",
                principalColumn: "Id");
        }
    }
}
