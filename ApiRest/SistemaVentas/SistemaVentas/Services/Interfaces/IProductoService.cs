using SistemaVentas.Models;

namespace SistemaVentas.Services.Interfaces
{
    public interface IProductoService : IService<Producto>
    {
        Task<object?> GetLowStock(int threshold);
        Task UpdateStock(int id, int cantidad);
    }
}
