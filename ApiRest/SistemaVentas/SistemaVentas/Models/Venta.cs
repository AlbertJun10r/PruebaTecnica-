using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public decimal Total { get; set; }

        public ICollection<VentasProducto> VentasProductos { get; set; }
    }
}