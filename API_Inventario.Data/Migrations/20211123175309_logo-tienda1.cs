using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Inventario.Data.Migrations
{
    public partial class logotienda1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo_url",
                table: "T_Tienda",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo_url",
                table: "T_Tienda");
        }
    }
}
