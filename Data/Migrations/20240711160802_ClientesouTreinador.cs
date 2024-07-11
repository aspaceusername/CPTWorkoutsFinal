using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CPTWorkouts.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientesouTreinador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TreinadorID",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreinadorID",
                table: "Utilizadores");
        }
    }
}
