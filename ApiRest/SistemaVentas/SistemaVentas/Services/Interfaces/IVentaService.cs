using SistemaVentas.DTOs;
using SistemaVentas.DTOs.VentaDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Services.Interfaces
{
    public interface IVentaService
    {
        Task<IEnumerable<VentaDTO>> GetAllAsync();
        Task<VentaDTO> GetByIdAsync(int id);
        Task<VentaDTO> CreateAsync(VentaCreateDTO ventaCreateDTO);
        Task UpdateAsync(int id, VentaUpdateDTO ventaUpdateDTO);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<VentaProductoDTO>> GetProductosByVentaIdAsync(int ventaId);
    }
}