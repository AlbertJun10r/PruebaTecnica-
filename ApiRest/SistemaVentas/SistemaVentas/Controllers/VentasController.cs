using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.DTOs;
using SistemaVentas.Models;
using SistemaVentas.Services.Interfaces;

namespace SistemaVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : BaseController<Venta>
    {
        private readonly IVentaService _ventaService;
        private readonly IMapper _mapper;

        public VentasController(IVentaService service, IMapper mapper) : base(service)
        {
            _ventaService = service;
            _mapper = mapper;
        }

        // Eliminado GetAllVentas() para evitar conflicto con BaseController.GetAll()

        [HttpGet("{id}/productos")]
        public async Task<ActionResult<IEnumerable<VentasProducto>>> GetProductosByVenta(int id)
        {
            // Primero valida el ID
            if (id <= 0) return BadRequest("ID inválido");

            // Luego busca la venta
            var venta = await _ventaService.GetVentaWithProductosAsync(id); // Cambiar el método para que devuelva un Task<Venta>
            if (venta == null) return NotFound();

            return Ok(venta.VentasProductos);
        }

        [HttpPost("create")]
        public async Task<ActionResult<VentaDTO>> CreateVenta([FromBody] VentaCreateDTO ventaCreateDTO)
        {
            // Validación básica
            if (ventaCreateDTO.ProductoIds.Count != ventaCreateDTO.Cantidades.Count)
                return BadRequest("El número de productos y cantidades no coincide.");

            try
            {
                var venta = _mapper.Map<Venta>(ventaCreateDTO);
                var createdVenta = await _ventaService.CreateVenta(venta, ventaCreateDTO);
                var ventaDTO = _mapper.Map<VentaDTO>(createdVenta);

                return CreatedAtAction(nameof(GetById), new { id = ventaDTO.Id }, ventaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Los demás métodos (GetById, Delete) se heredan de BaseController
    }
}