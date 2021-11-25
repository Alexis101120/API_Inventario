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
    public class InventariosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<InventariosController> _logger;

        public InventariosController(IContenedorTrabajo ctx, IMapper mapper, ILogger<InventariosController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{InventarioId:int}")]
        public async Task<IActionResult> Get(int InventarioId)
        {
            try
            {
                var Inventario = await _ctx.Inventario.GetFirstOrdefaultAsync(x => x.Id == InventarioId, include:source=> source.Include(x=> x.Usuario));
                var InventarioDto = _mapper.Map<InventarioDTO>(Inventario);
                InventarioDto.Usuario = Inventario.Usuario.Nombre_Completo;
                return Ok(new { succesS = true, data = InventarioDto });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("ObtenTodo/{TiendaId:int}")]
        public async Task<IActionResult> ObtenTodo(int TiendaId)
        {
            try
            {
                var Inventarios = await _ctx.Inventario.GetAllasync(x => x.Tienda_id == TiendaId, include: source=> source.Include(x=> x.Usuario));
                var Inventarios_DTO = _mapper.Map<List<InventarioDTO>>(Inventarios);
                return Ok(new { success = true, data = Inventarios_DTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("Abrir/{InventarioId:int}")]
        public async Task<IActionResult> Abrir(int InventarioId)
        {
            try
            {
                var Result = await _ctx.Inventario.Abrir(InventarioId);
                await _ctx.SaveAsync();
                if(Result == true)
                {
                    return Ok(new { success = true, mensaje = "Inventario abierto" });
                }
                else
                {
                    return Ok(new { success = false, mensaje = "Ocurrio un error, intentelo de nuevo" });
                }
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("Cerrar/{InventarioId:int}")]
        public async Task<IActionResult> Cerrar(int InventarioId)
        {
            try
            {
                var Result = await _ctx.Inventario.Cerrar(InventarioId);
                await _ctx.SaveAsync();
                if (Result == true)
                {
                    return Ok(new { success = true, mensaje = "Inventario abierto" });
                }
                else
                {
                    return Ok(new { success = false, mensaje = "Ocurrio un error, intentelo de nuevo" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Inventario_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    var Inventario = _mapper.Map<T_Inventario>(item);
                    Inventario.Usuario_Id = usuarioId;
                    await _ctx.Inventario.AddAsync(Inventario);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Inventario creado con éxito" });
                }
                return Ok(new { success = false, mensaje = "Error al crear inventrario" });
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        
    }
}
