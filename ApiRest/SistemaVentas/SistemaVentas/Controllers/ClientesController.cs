using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : BaseController<Cliente>
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClientesController(IClienteService clienteService, IMapper mapper) : base(clienteService)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet("clienteDTO")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            var clientesDTO = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);
            return Ok(clientes);
        }

        [HttpGet("{id}/cliente")]
        public override async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound();

            var clienteDTO = _mapper.Map<ClienteDTO>(cliente);
            return Ok(clienteDTO);
        }

        [HttpPost("clienteDTO")]
        public async Task<ActionResult<ClienteDTO>> Create([FromBody] ClienteCreateDTO clienteCreateDTO)
        {
            var cliente = _mapper.Map<Cliente>(clienteCreateDTO);
            var createdCliente = await _clienteService.CreateAsync(cliente); // Usa el método corregido
            var clienteDTO = _mapper.Map<ClienteDTO>(createdCliente);
            return CreatedAtAction(nameof(GetById), new { id = clienteDTO.Id }, clienteDTO);
        }
    }
}
