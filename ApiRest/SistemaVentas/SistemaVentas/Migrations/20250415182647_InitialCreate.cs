using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaVentas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__3214EC2765A7ECB6", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__3214EC27D9BA2781", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    cliente_ID = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas__3214EC27669E1830", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Ventas__cliente___3D5E1FD2",
                        column: x => x.cliente_ID,
                        principalTable: "Clientes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ventas_Productos",
                columns: table => new
                {
                    venta_ID = table.Column<int>(type: "int", nullable: false),
                    producto_ID = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ventas_P__EC2B685FD31C4B95", x => new { x.venta_ID, x.producto_ID });
                    table.ForeignKey(
                        name: "FK__Ventas_Pr__produ__4222D4EF",
                        column: x => x.producto_ID,
                        principalTable: "Productos",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__Ventas_Pr__venta__412EB0B6",
                        column: x => x.venta_ID,
                        principalTable: "Ventas",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_cliente_ID",
                table: "Ventas",
                column: "cliente_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_Productos_producto_ID",
                table: "Ventas_Productos",
                column: "producto_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ventas_Productos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
