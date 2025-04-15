using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Services
{
    public class ClienteService : IClienteService
    {
        private readonly PruebaTecnicaContext _context;
        private readonly IMapper _mapper;

        public ClienteService(PruebaTecnicaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            var clientes = await _context.Clientes.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<ClienteDTO>>(clientes);
        }

        public async Task<ClienteDTO> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> CreateAsync(ClienteCreateDTO clienteCreateDTO)
        {
            var cliente = _mapper.Map<Cliente>(clienteCreateDTO);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task UpdateAsync(int id, ClienteUpdateDTO clienteUpdateDTO)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");

            _mapper.Map(clienteUpdateDTO, cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}