using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.DTOs;
using SistemaVentas.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClientesController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> Create([FromBody] ClienteCreateDTO clienteCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cliente = await _clienteService.CreateAsync(clienteCreateDTO);
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDTO clienteUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _clienteService.UpdateAsync(id, clienteUpdateDTO);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clienteService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}