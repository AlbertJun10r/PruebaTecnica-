using System;
using System.Collections.Generic;

namespace SistemaVentas.DTOs
{
    public class VentaUpdateDTO
    {
        public DateTime? Fecha { get; set; }
        public int? ClienteId { get; set; }

        //public List<VentaProductoUpdateDTO> Productos { get; set; }
    }
}