using AutoMapper;
using SistemaVentas.DTOs;
using SistemaVentas.DTOs.VentaDTOs;
using SistemaVentas.Models;

namespace SistemaVentas.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente
            CreateMap<ClienteCreateDTO, Cliente>();
            CreateMap<ClienteUpdateDTO, Cliente>();
            CreateMap<Cliente, ClienteDTO>();

            // Producto
            CreateMap<ProductoCreateDTO, Producto>();
            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<Producto, ProductoDTO>();

            // Venta
            CreateMap<Venta, VentaDTO>();
            CreateMap<VentasProducto, VentaProductoDTO>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.PrecioUnitario * src.Cantidad));
        }
    }
}