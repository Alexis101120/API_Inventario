using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UsuarioDTO, T_Usuario>().ReverseMap();
            CreateMap<Usuario_CrearDTO, T_Usuario>();
            CreateMap<TiendaDTO, T_Tienda>().ForMember(x=> x.Logo_url, options => options.MapFrom(x=> x.Logo_url)).ReverseMap();
            CreateMap<Tienda_CrearDTO, T_Tienda>().ReverseMap();
            CreateMap<ProductoDTO, T_Producto>().ReverseMap();
            CreateMap<T_Inventario, InventarioDTO>().ForMember(x=> x.Usuario , options=> options.MapFrom(x=> x.Usuario.Nombre_Completo));
            CreateMap<Inventario_CrearDTO, T_Inventario>();
            CreateMap<T_Movimientos_Inventario, MovimientoInventarioDTO>().ForMember(x => x.Descripcion, options => options.MapFrom(x => x.Producto.Descripcion)).ForMember(x=> x.Usuario, options=> options.MapFrom(x=> x.Usuario.Nombre_Completo));
            CreateMap<Movimiento_Inventario_CrearDTO, T_Movimientos_Inventario>();
            CreateMap<T_Producto_Inventario, Producto_Inventario_DTO>().ForMember(x => x.Descripcion, options => options.MapFrom(x => x.Producto.Descripcion));
        }
    }
}
