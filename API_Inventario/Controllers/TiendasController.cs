using API_Inventario.Data.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendasController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
       

    }
}
