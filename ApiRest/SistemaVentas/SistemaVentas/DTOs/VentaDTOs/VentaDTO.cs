using System.Collections.Generic;

namespace SistemaVentas.DTOs.VentaDTOs
{
    public class VentaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public ClienteDTO Cliente { get; set; }
        public decimal Total { get; set; }
        public List<VentaProductoDTO> Productos { get; set; }
    }
}