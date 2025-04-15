using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.DTOs;
using SistemaVentas.DTOs.VentaDTOs;
using SistemaVentas.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;
        private readonly IMapper _mapper;

        public VentasController(IVentaService ventaService, IMapper mapper)
        {
            _ventaService = ventaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> GetAll()
        {
            var ventas = await _ventaService.GetAllAsync();
            return Ok(ventas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDTO>> GetById(int id)
        {
            var venta = await _ventaService.GetByIdAsync(id);
            if (venta == null) return NotFound();
            return Ok(venta);
        }

        [HttpGet("{id}/productos")]
        public async Task<ActionResult<IEnumerable<VentaProductoDTO>>> GetProductosByVenta(int id)
        {
            var productos = await _ventaService.GetProductosByVentaIdAsync(id);
            return Ok(productos);
        }

        [HttpPost]
        public async Task<ActionResult<VentaDTO>> Create([FromBody] VentaCreateDTO ventaCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (ventaCreateDTO.ProductoIds.Count != ventaCreateDTO.Cantidades.Count)
                return BadRequest("La cantidad de productos no coincide con las cantidades");

            try
            {
                var venta = await _ventaService.CreateAsync(ventaCreateDTO);
                return CreatedAtAction(nameof(GetById), new { id = venta.Id }, venta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VentaUpdateDTO ventaUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _ventaService.UpdateAsync(id, ventaUpdateDTO);
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
            var result = await _ventaService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}