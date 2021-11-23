using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Inventario.Data.Migrations
{
    public partial class mjwtUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "T_Movimientos_Inventarios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Expiration",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T_Productos_Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Inventario_Id = table.Column<int>(type: "int", nullable: false),
                    Existencia = table.Column<int>(type: "int", nullable: false),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Productos_Inventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Productos_Inventario_T_Inventarios_Inventario_Id",
                        column: x => x.Inventario_Id,
                        principalTable: "T_Inventarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_Productos_Inventario_T_Productos_Codigo",
                        column: x => x.Codigo,
                        principalTable: "T_Productos",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Movimientos_Inventarios_Codigo",
                table: "T_Movimientos_Inventarios",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_T_Productos_Inventario_Codigo",
                table: "T_Productos_Inventario",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_T_Productos_Inventario_Inventario_Id",
                table: "T_Productos_Inventario",
                column: "Inventario_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Movimientos_Inventarios_T_Productos_Codigo",
                table: "T_Movimientos_Inventarios",
                column: "Codigo",
                principalTable: "T_Productos",
                principalColumn: "Codigo",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Movimientos_Inventarios_T_Productos_Codigo",
                table: "T_Movimientos_Inventarios");

            migrationBuilder.DropTable(
                name: "T_Productos_Inventario");

            migrationBuilder.DropIndex(
                name: "IX_T_Movimientos_Inventarios_Codigo",
                table: "T_Movimientos_Inventarios");

            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "T_Movimientos_Inventarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
