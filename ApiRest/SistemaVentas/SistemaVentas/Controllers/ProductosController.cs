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
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;

        public ProductosController(IProductoService productoService, IMapper mapper)
        {
            _productoService = productoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Create([FromBody] ProductoCreateDTO productoCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var producto = await _productoService.CreateAsync(productoCreateDTO);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDTO productoUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productoService.UpdateAsync(id, productoUpdateDTO);
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
            var result = await _productoService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetLowStock([FromQuery] int threshold = 5)
        {
            var productos = await _productoService.GetLowStockAsync(threshold);
            return Ok(productos);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int cantidad)
        {
            try
            {
                await _productoService.UpdateStockAsync(id, cantidad);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}