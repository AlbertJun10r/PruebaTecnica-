using SistemaVentas.DTOs;
using SistemaVentas.Models;

namespace SistemaVentas.Services.Interfaces
{
    public interface IVentaService : IService<Venta>
    {
        Task<Venta> CreateVenta(Venta venta, VentaCreateDTO ventaCreateDTO);
        Task<IEnumerable<VentasProducto>> GetProductosByVentaID(int ventaID);
        Task <Venta> GetVentaWithProductosAsync(int id);
    }
}
