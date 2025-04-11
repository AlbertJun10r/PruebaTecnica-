using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : BaseController<Producto>
    {
        private readonly IProductoService _productoService;
        private readonly PruebaTecnicaContext _context;
        private readonly IMapper _mapper;
        public ProductosController(IProductoService service,IMapper mapper, PruebaTecnicaContext context) : base(service)
        {
            _productoService = service;
            _context = context;
            _mapper = mapper;
        }

        // Corrected return type to match the base class method signature
        public override async Task<IActionResult> GetAll()
        {
            var productos = await _context.Productos.Include(p => p.VentasProductos).ToListAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var cliente = await _productoService.GetByIdAsync(id);
            if (cliente == null) return NotFound();

            var clienteDTO = _mapper.Map<ProductoDTO>(cliente);
            return Ok(clienteDTO);
        }

        // Endpoint adicional
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetLowStock([FromQuery] int threshold = 5)
        {
            return Ok(await _productoService.GetLowStock(threshold));
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int newStock)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            producto.Stock = newStock;
            await _productoService.UpdateAsync(id, producto);
            return NoContent();
        }
    }
}
