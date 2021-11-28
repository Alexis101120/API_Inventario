using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductosInventarioController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosInventarioController> _logger;

        public ProductosInventarioController(IContenedorTrabajo ctx, IMapper mapper, ILogger<ProductosInventarioController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("ObtenTodos/{InventarioId:int}")]
        public async Task<IActionResult> ObtenTodos(int InventarioId)
        {
            try
            {
                var Productos = await _ctx.Producto_Inventario.GetAllasync(x => x.Inventario_Id == InventarioId, include:source=> source.Include(x=> x.Producto));
                var ProductosDTO = _mapper.Map<List<Producto_Inventario_DTO>>(Productos);
                return Ok(new { success = true, data = ProductosDTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

    }
}
