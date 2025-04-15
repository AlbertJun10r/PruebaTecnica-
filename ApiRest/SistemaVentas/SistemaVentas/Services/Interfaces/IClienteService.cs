using SistemaVentas.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> GetAllAsync();
        Task<ClienteDTO> GetByIdAsync(int id);
        Task<ClienteDTO> CreateAsync(ClienteCreateDTO clienteCreateDTO);
        Task UpdateAsync(int id, ClienteUpdateDTO clienteUpdateDTO);
        Task<bool> DeleteAsync(int id);
    }
}