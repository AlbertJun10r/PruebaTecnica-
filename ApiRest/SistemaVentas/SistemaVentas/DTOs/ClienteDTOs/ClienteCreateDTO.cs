using System.ComponentModel.DataAnnotations;

namespace SistemaVentas.DTOs
{
    public class ClienteCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }
    }
}