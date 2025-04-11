using AutoMapper;
using SistemaVentas.DTOs;
using SistemaVentas.Models;

namespace PruebaTecnica.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteCreateDTO, Cliente>();

            // Producto
            CreateMap<Producto, ProductoDTO>();
            CreateMap<ProductoCreateDTO, Producto>();

            // Venta
            CreateMap<Venta, VentaDTO>();
            CreateMap<VentaCreateDTO, Venta>();
            CreateMap<VentasProducto, VentaProductoDTO>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.PrecioUnitario * src.Cantidad));
        }
           
    }
}
