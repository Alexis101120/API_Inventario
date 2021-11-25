using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Inventario.Data.Migrations
{
    public partial class TiendaNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "T_Inventarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "T_Inventarios");
        }
    }
}
