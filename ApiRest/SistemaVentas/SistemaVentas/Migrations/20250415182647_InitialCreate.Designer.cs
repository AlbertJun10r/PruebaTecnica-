﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaVentas.Data;

#nullable disable

namespace SistemaVentas.Migrations
{
    [DbContext(typeof(PruebaTecnicaContext))]
    [Migration("20250415182647_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombre");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("telefono");

                    b.HasKey("Id")
                        .HasName("PK__Clientes__3214EC2765A7ECB6");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("SistemaVentas.Models.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("descripcion");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nombre");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("precio");

                    b.Property<int>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.HasKey("Id")
                        .HasName("PK__Producto__3214EC27D9BA2781");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("SistemaVentas.Models.Venta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("cliente_ID");

                    b.Property<DateTime>("Fecha")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("fecha")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("total");

                    b.HasKey("Id")
                        .HasName("PK__Ventas__3214EC27669E1830");

                    b.HasIndex("ClienteId");

                    b.ToTable("Ventas");
                });

            modelBuilder.Entity("SistemaVentas.Models.VentasProducto", b =>
                {
                    b.Property<int>("VentaId")
                        .HasColumnType("int")
                        .HasColumnName("venta_ID");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int")
                        .HasColumnName("producto_ID");

                    b.Property<int>("Cantidad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("cantidad");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("precio_unitario");

                    b.HasKey("VentaId", "ProductoId")
                        .HasName("PK__Ventas_P__EC2B685FD31C4B95");

                    b.HasIndex("ProductoId");

                    b.ToTable("Ventas_Productos", (string)null);
                });

            modelBuilder.Entity("SistemaVentas.Models.Venta", b =>
                {
                    b.HasOne("Cliente", "Cliente")
                        .WithMany("Venta")
                        .HasForeignKey("ClienteId")
                        .IsRequired()
                        .HasConstraintName("FK__Ventas__cliente___3D5E1FD2");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("SistemaVentas.Models.VentasProducto", b =>
                {
                    b.HasOne("SistemaVentas.Models.Producto", "Producto")
                        .WithMany("VentasProductos")
                        .HasForeignKey("ProductoId")
                        .IsRequired()
                        .HasConstraintName("FK__Ventas_Pr__produ__4222D4EF");

                    b.HasOne("SistemaVentas.Models.Venta", "Venta")
                        .WithMany("VentasProductos")
                        .HasForeignKey("VentaId")
                        .IsRequired()
                        .HasConstraintName("FK__Ventas_Pr__venta__412EB0B6");

                    b.Navigation("Producto");

                    b.Navigation("Venta");
                });

            modelBuilder.Entity("Cliente", b =>
                {
                    b.Navigation("Venta");
                });

            modelBuilder.Entity("SistemaVentas.Models.Producto", b =>
                {
                    b.Navigation("VentasProductos");
                });

            modelBuilder.Entity("SistemaVentas.Models.Venta", b =>
                {
                    b.Navigation("VentasProductos");
                });
#pragma warning restore 612, 618
        }
    }
}
