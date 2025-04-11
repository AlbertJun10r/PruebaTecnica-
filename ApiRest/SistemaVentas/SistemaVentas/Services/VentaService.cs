using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Services
{
    public class VentaService : BaseService<Venta>, IVentaService
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;
        private readonly PruebaTecnicaContext _context;

        public VentaService(PruebaTecnicaContext context, IMapper mapper, IProductoService productoService) : base(context)
        {
            _productoService = productoService;
            _context = context;
            _mapper = mapper;
        }

        public new async Task<IEnumerable<VentaDTO>> GetAllAsync()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.VentasProductos)
                .ThenInclude(vp => vp.Producto)
                .ToListAsync();

            return _mapper.Map<IEnumerable<VentaDTO>>(ventas);
        }

        // Para obtener DTOs (usado por API externas)
        public new async Task<VentaDTO> GetByIdAsync(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.VentasProductos)
                .ThenInclude(vp => vp.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            return _mapper.Map<VentaDTO>(venta);
        }

        public async Task<Venta> CreateVenta(Venta venta, VentaCreateDTO ventaCreateDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuevaVenta = new Venta // Renamed variable to 'nuevaVenta' to avoid conflict
                {
                    ClienteId = ventaCreateDTO.ClienteId,
                    Fecha = DateTime.Now,
                    Total = 0
                };

                _context.Ventas.Add(nuevaVenta);
                await _context.SaveChangesAsync();

                decimal total = 0;
                for (int i = 0; i < ventaCreateDTO.ProductoIds.Count; i++)
                {
                    var productoId = ventaCreateDTO.ProductoIds[i];
                    var cantidad = ventaCreateDTO.Cantidades[i];

                    var producto = await _productoService.GetByIdAsync(productoId);
                    if (producto == null || producto.Stock < cantidad)
                    {
                        throw new Exception($"Producto {productoId} no disponible o stock insuficiente");
                    }

                    var subtotal = producto.Precio * cantidad;
                    total += subtotal;

                    var ventaProducto = new VentasProducto
                    {
                        VentaId = nuevaVenta.Id,
                        ProductoId = productoId,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio
                    };

                    _context.VentasProductos.Add(ventaProducto);
                    await _productoService.UpdateStock(productoId, -cantidad);
                }

                nuevaVenta.Total = total;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return nuevaVenta;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Para obtener ventas con productos (usado por el controlador)
        public async Task<Venta> GetVentaWithProductosAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.VentasProductos)
                .ThenInclude(vp => vp.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VentasProducto>> GetProductosByVentaID(int ventaId)
        {
            return await _context.VentasProductos
                .Include(vp => vp.Producto)
                .Where(vp => vp.VentaId == ventaId)
                .ToListAsync();
        }
    }
}