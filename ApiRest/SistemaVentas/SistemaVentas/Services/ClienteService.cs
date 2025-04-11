using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Services
{
    public class ClienteService : BaseService<Cliente>, IClienteService
    {
        private readonly PruebaTecnicaContext _context;
        private readonly IMapper _mapper;
        public ClienteService(PruebaTecnicaContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<ClienteDTO> GetClienteDTOByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            return cliente == null ? null : _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> CreateAsync(ClienteDTO clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return _mapper.Map<ClienteDTO>(cliente);
        }
    }

}
