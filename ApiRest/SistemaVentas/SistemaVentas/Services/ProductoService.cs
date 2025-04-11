using SistemaVentas.Data;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Services
{
    public class ProductoService : BaseService<Producto>, IProductoService
    {
        private readonly PruebaTecnicaContext _context;
        public ProductoService(PruebaTecnicaContext context) : base(context)
        {
            _context = context;
        }

        public Task<object?> GetLowStock(int threshold)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Producto> GetByIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task UpdateStock(int id, int cantidad)
        {
            var producto = await GetByIdAsync(id);
            if (producto != null) // Ensure producto is not null
            {
                producto.Stock += cantidad;
                await UpdateAsync(id, producto);
            }
        }
    }
}
