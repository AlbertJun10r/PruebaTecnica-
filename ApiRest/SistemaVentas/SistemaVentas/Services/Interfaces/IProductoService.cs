using SistemaVentas.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> GetAllAsync();
        Task<ProductoDTO> GetByIdAsync(int id);
        Task<ProductoDTO> CreateAsync(ProductoCreateDTO productoCreateDTO);
        Task UpdateAsync(int id, ProductoUpdateDTO productoUpdateDTO);
        Task<bool> DeleteAsync(int id);
        Task UpdateStockAsync(int id, int cantidad);
        Task<IEnumerable<ProductoDTO>> GetLowStockAsync(int threshold);
    }
}