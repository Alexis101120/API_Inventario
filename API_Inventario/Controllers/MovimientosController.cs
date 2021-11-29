using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
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
                var Movimiento = await _ctx.Movimiento_Inventario.GetFirstOrdefaultAsync(x => x.Id == MovimientoId,include: source=> source.Include(x=> x.Usuario));
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
                var Movimientos = await _ctx.Movimiento_Inventario.GetAllasync(x => x.Inventario_Id == InventarioId, include: source=> source.Include(x=> x.Producto).Include(x=> x.Usuario));
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
                var Inventario = await _ctx.Inventario.GetFirstOrdefaultAsync(x => x.Id == item.Inventario_Id);
                Movimiento.Usuario_Id = usuarioId;
                await _ctx.Movimiento_Inventario.AddAsync(Movimiento);
                var Producto_Cat = await _ctx.Producto.GetFirstOrdefaultAsync(x => x.Codigo == item.Codigo);
                if(Producto_Cat == null)
                {
                    Producto_Cat = new T_Producto
                    {
                        Codigo = item.Codigo,
                        Descripcion = item.Descripcion,
                        Tienda_Id = Inventario.Tienda_id
                    };
                    await _ctx.Producto.AddAsync(Producto_Cat);
                    await _ctx.SaveAsync();
                }
                var Producto = await _ctx.Producto_Inventario.GetFirstOrdefaultAsync(x => x.Codigo == item.Codigo && x.Inventario_Id==item.Inventario_Id);
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

        [HttpPut("{MovimientoId:int}")]
        public async Task<IActionResult> Put(int MovimientoId, [FromBody] Movimiento_Inventario_CrearDTO item)
        {
            try
            {
                var Inventario = _mapper.Map<T_Movimientos_Inventario>(item);
                Inventario.Id = MovimientoId;
                await _ctx.Movimiento_Inventario.Update(Inventario);
                await _ctx.SaveAsync();
                return Ok(new { success = true, mensaje = "Movimiento actualizado con éxito" });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpDelete("{MovimientoId:int}")]
        public async Task<IActionResult> Delete(int MovimientoId)
        {
            try
            {
                await _ctx.Movimiento_Inventario.SoftDelete(MovimientoId);
                await _ctx.SaveAsync();
                return Ok(new { success=true, mensaje="Movimiento eliminado con éxito" });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }
        
    }
}
