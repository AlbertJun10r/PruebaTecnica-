using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentas.Services
{
    public class ProductoService : IProductoService
    {
        private readonly PruebaTecnicaContext _context;
        private readonly IMapper _mapper;

        public ProductoService(PruebaTecnicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<ProductoDTO>>(productos);
        }

        public async Task<ProductoDTO> GetByIdAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            return _mapper.Map<ProductoDTO>(producto);
        }

        public async Task<ProductoDTO> CreateAsync(ProductoCreateDTO productoCreateDTO)
        {
            var producto = _mapper.Map<Producto>(productoCreateDTO);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductoDTO>(producto);
        }

        public async Task UpdateAsync(int id, ProductoUpdateDTO productoUpdateDTO)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            _mapper.Map(productoUpdateDTO, producto);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateStockAsync(int id, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            producto.Stock += cantidad;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductoDTO>> GetLowStockAsync(int threshold)
        {
            var productos = await _context.Productos
                .Where(p => p.Stock <= threshold)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductoDTO>>(productos);
        }
    }
}