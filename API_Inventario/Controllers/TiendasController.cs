using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using API_Inventario.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TiendasController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<TiendasController> _logger;
        private readonly IAlmacenadorArchivos _almacenador;


        public TiendasController(IContenedorTrabajo ctx, IMapper mapper, ILogger<TiendasController> logger, IAlmacenadorArchivos almacenador)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
            _almacenador = almacenador;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Tiendas = await _ctx.Tienda.GetAllasync();
                var TiendasDTO = _mapper.Map<List<TiendaDTO>>(Tiendas);
                return Ok(new { success = true, data = TiendasDTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("{TiendaId:int}")]
        public async Task<IActionResult> Get(int TiendaId)
        {
            try
            {
                var Tienda = await _ctx.Tienda.GetFirstOrdefaultAsync(x => x.Id == TiendaId);
                var TiendaDTO = _mapper.Map<TiendaDTO>(Tienda);
                return Ok(new { success = true, data = TiendaDTO });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Tienda_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Tienda = _mapper.Map<T_Tienda>(item);
                    if (item.Logo != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await item.Logo.CopyToAsync(memoryStream);
                            var contenido = memoryStream.ToArray();
                            var extension = Path.GetExtension(item.Logo.FileName);
                            var Carpeta = @"Imagenes\Tiendas";
                            Tienda.Logo_url = await _almacenador.GuardarArchivo(contenido, extension, Carpeta, item.Logo.ContentType);
                        }
                    }
                    await _ctx.Tienda.AddAsync(Tienda);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Tienda registrada con éxito" });
                }
                string Errores = "";
                var Errores_Modelo = ModelState.Values;
                foreach(var Values in Errores_Modelo)
                {
                   foreach(var Error in Values.Errors)
                    {
                        Errores += Error.ErrorMessage;
                        Errores += Environment.NewLine;
                    }
                }
                return BadRequest(new { success = false, mensaje = Errores });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPut("{TiendaId:int}")]
        public async Task<IActionResult> Put(int TiendaId, [FromBody] Tienda_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Tienda = _mapper.Map<T_Tienda>(item);
                    Tienda.Id = TiendaId;
                    await _ctx.Tienda.Update(Tienda);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Tienda actualizada con éxito" });
                }
                string Errores = "";
                var Errores_Modelo = ModelState.Values;
                foreach (var Values in Errores_Modelo)
                {
                    foreach (var Error in Values.Errors)
                    {
                        Errores += Error.ErrorMessage;
                        Errores += Environment.NewLine;
                    }
                }
                return BadRequest(new { success = false, mensaje = Errores });
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpDelete("{TiendaId:int}")]
        public async Task<IActionResult> Delete(int TiendaId)
        {
            _ctx.Tienda.Remove(TiendaId);
            await _ctx.SaveAsync();
            return Ok(new { success = true });
        }
    }
}
