﻿namespace SistemaVentas.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class ProductoCreateDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

    public class ProductoUpdateStockDTO
    {
        public int Cantidad { get; set; }
    }
}
