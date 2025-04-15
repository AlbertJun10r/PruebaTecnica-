using System.Collections.Generic;

namespace SistemaVentas.DTOs
{
    public class VentaCreateDTO
    {
        public int ClienteId { get; set; }
        public List<int> ProductoIds { get; set; }
        public List<int> Cantidades { get; set; }
    }
}