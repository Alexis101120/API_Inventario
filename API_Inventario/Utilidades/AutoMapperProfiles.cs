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
            CreateMap<T_Usuario, UsuarioDTO>();
            CreateMap<Usuario_CrearDTO, T_Usuario>();
            CreateMap<T_Tienda, TiendaDTO>();
            CreateMap<Tienda_CrearDTO, T_Tienda>();
            CreateMap<T_Inventario, InventarioDTO>();
            CreateMap<Inventario_CrearDTO, T_Inventario>();
            CreateMap<T_Producto, ProductoDTO>();
            CreateMap<Producto_CrearDTO, T_Producto>();
        }
    }
}
