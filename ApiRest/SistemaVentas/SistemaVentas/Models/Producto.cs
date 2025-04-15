using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public virtual ICollection<VentasProducto> VentasProductos { get; set; } = new List<VentasProducto>();
    }
}
