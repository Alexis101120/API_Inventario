using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MovimientosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<MovimientosController> _logger;

        public MovimientosController(IContenedorTrabajo ctx, IMapper mapper, ILogger<MovimientosController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{MovimientoId:int}")]
        public async Task<IActionResult> Get(int MovimientoId)
        {
            try
            {
                var Movimiento = await _ctx.Movimiento_Inventario.GetFirstOrdefaultAsync(x => x.Id == MovimientoId);
                var MovimientoDTO = _mapper.Map<MovimientoInventarioDTO>(Movimiento);
                return Ok(new { success = true, data = MovimientoDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("ObtenTodos/{InventarioId:int}")]
        public async Task<IActionResult> ObtenTodos (int InventarioId)
        {
            try
            {
                var Movimientos = await _ctx.Movimiento_Inventario.GetAllasync(x => x.Inventario_Id == InventarioId);
                var MovimientosDTO = _mapper.Map<List<MovimientoInventarioDTO>>(Movimientos);
                return Ok(new { success = true, data = MovimientosDTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movimiento_Inventario_CrearDTO item)
        {
            try
            {
                var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var Movimiento = _mapper.Map<T_Movimientos_Inventario>(item);
                Movimiento.Usuario_Id = usuarioId;
                await _ctx.Movimiento_Inventario.AddAsync(Movimiento);
                var Producto = await _ctx.Producto_Inventario.GetFirstOrdefaultAsync(x => x.Codigo == item.Codigo);
                if(Producto == null)
                {
                    T_Producto_Inventario Producto_inv = new T_Producto_Inventario
                    {
                        Codigo = item.Codigo,
                        Inventario_Id = item.Inventario_Id,
                        Existencia = item.Cantidad,
                    };
                    await _ctx.Producto_Inventario.AddAsync(Producto_inv);
                    await _ctx.SaveAsync();
                }
                else
                {
                    await _ctx.Producto_Inventario.Update(Producto, item.Cantidad);
                    await _ctx.SaveAsync();
                }
                return Ok(new { success = true, mensaje = "Movimiento registrado con éxito" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });

            }
        }
    }
}
