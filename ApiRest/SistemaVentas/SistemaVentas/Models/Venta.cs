using System;
using System.Collections.Generic;

namespace SistemaVentas.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int ClienteId { get; set; }

    public decimal Total { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<VentasProducto> VentasProductos { get; set; } = new List<VentasProducto>();
}
