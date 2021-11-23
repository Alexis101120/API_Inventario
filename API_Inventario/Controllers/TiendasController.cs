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

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendasController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;

        public TiendasController(IContenedorTrabajo ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet("{TiendaId:int}")]
        public async Task<IActionResult> Get(int TiendaId)
        {
            try
            {
                var Tienda = await _ctx.Tienda.GetFirstOrdefaultAsync(x => x.Id == TiendaId);
                var TiendaDTO = _mapper.Map<TiendaDTO>(Tienda);
                return Ok(new { success = true, data = TiendaDTO });
            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Tiendas = await _ctx.Tienda.GetAllasync();
                var TiendasDTO = _mapper.Map<List<TiendaDTO>>(Tiendas);
                return Ok(new { success = true, data = TiendasDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Tienda_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Tienda = _mapper.Map<T_Tienda>(item);
                    await _ctx.Tienda.AddAsync(Tienda);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, data = "Tienda agregada con éxito" });
                }
                return BadRequest(new { success = false, data = "Error al registrar tienda, verifique sus datos"  });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPut("{TiendaId:int}")]
        public async Task<IActionResult> Put(int TiendaId, [FromBody]Tienda_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Tienda = _mapper.Map<T_Tienda>(item);
                    Tienda.Id = TiendaId;
                    await _ctx.Tienda.Update(Tienda);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, data = "Tienda editada con éxito" });
                }
                return BadRequest(new { success = false, data = "Error al registrar tienda, verifique sus datos" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpDelete("{TiendaId:int}")]
        public async Task<IActionResult> Delete(int TiendaId)
        {
            try
            {
                await _ctx.Tienda.SoftDelete(TiendaId);
                await _ctx.SaveAsync();
                return Ok(new { success = true, data = "Tienda eliminada con éxito" });
            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

    }
}
