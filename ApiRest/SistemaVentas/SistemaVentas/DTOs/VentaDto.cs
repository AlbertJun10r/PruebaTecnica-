namespace SistemaVentas.DTOs
{
    public class VentaDTO
    {
        public int Id { get; set; }
        public System.DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public decimal Total { get; set; }
        public ClienteDTO Cliente { get; set; }
        public List<VentaProductoDTO> VentasProductos { get; set; }
    }

    public class VentaCreateDTO
    {
        public int ClienteId { get; set; }
        public List<int> ProductoIds { get; set; }
        public List<int> Cantidades { get; set; }
    }

    public class VentaProductoDTO
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
