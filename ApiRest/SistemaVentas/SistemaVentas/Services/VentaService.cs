using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.DTOs.VentaDTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentas.Services
{
    public class VentaService : IVentaService
    {
        private readonly PruebaTecnicaContext _context;
        private readonly IMapper _mapper;
        private readonly IProductoService _productoService;

        public VentaService(
            PruebaTecnicaContext context,
            IMapper mapper,
            IProductoService productoService)
        {
            _context = context;
            _mapper = mapper;
            _productoService = productoService;
        }

        public async Task<IEnumerable<VentaDTO>> GetAllAsync()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.VentasProductos)
                    .ThenInclude(vp => vp.Producto)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<VentaDTO>>(ventas);
        }

        public async Task<VentaDTO> GetByIdAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.VentasProductos)
                    .ThenInclude(vp => vp.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            return _mapper.Map<VentaDTO>(venta);
        }

        public async Task<VentaDTO> CreateAsync(VentaCreateDTO ventaCreateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Venta
                {
                    ClienteId = ventaCreateDTO.ClienteId,
                    Fecha = DateTime.Now,
                    Total = 0
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                decimal total = 0;
                for (int i = 0; i < ventaCreateDTO.ProductoIds.Count; i++)
                {
                    var productoId = ventaCreateDTO.ProductoIds[i];
                    var cantidad = ventaCreateDTO.Cantidades[i];

                    var producto = await _productoService.GetByIdAsync(productoId);
                    if (producto == null || producto.Stock < cantidad)
                    {
                        throw new InvalidOperationException($"Producto {productoId} no disponible o stock insuficiente");
                    }

                    var subtotal = producto.Precio * cantidad;
                    total += subtotal;

                    var ventaProducto = new VentasProducto
                    {
                        VentaId = venta.Id,
                        ProductoId = productoId,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio
                    };

                    _context.VentasProductos.Add(ventaProducto);
                    await _productoService.UpdateStockAsync(productoId, -cantidad);
                }

                venta.Total = total;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(venta.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(int id, VentaUpdateDTO ventaUpdateDTO)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                throw new KeyNotFoundException($"Venta con ID {id} no encontrada");

            if (ventaUpdateDTO.Fecha.HasValue)
                venta.Fecha = ventaUpdateDTO.Fecha.Value;

            if (ventaUpdateDTO.ClienteId.HasValue)
                venta.ClienteId = ventaUpdateDTO.ClienteId.Value;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.VentasProductos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return false;

            // Restaurar stock
            foreach (var vp in venta.VentasProductos)
            {
                await _productoService.UpdateStockAsync(vp.ProductoId, vp.Cantidad);
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VentaProductoDTO>> GetProductosByVentaIdAsync(int ventaId)
        {
            var productos = await _context.VentasProductos
                .Include(vp => vp.Producto)
                .Where(vp => vp.VentaId == ventaId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VentaProductoDTO>>(productos);
        }
    }
}