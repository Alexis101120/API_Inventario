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
            CreateMap<TiendaDTO, T_Tienda>().ReverseMap();
            CreateMap<Tienda_CrearDTO, T_Tienda>().ReverseMap();
            CreateMap<ProductoDTO, T_Producto>().ReverseMap();
        }
    }
}
