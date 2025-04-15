using System.ComponentModel.DataAnnotations;
using SistemaVentas.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telefono { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
