using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IContenedorTrabajo ctx, IMapper mapper, ILogger<ProductosController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{Codigo}")]
        public async Task<IActionResult> Get(string Codigo)
        {
            try
            {
                var Producto = await _ctx.Producto.GetFirstOrdefaultAsync(x => x.Codigo == Codigo);
                var ProductoDTO = _mapper.Map<ProductoDTO>(Producto);
                return Ok(new { success = true, data = ProductoDTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("ObtenTodos/{TiendaId:int}")]
        public async Task<IActionResult> ObtenTodos(int TiendaId)
        {
            try
            {
                var Productos = await _ctx.Producto.GetAllasync(x => x.Tienda_Id == TiendaId);
                var ProductosDTO = _mapper.Map<List<ProductoDTO>>(Productos);
                return Ok(new { success = true, data = ProductosDTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Producto = _mapper.Map<T_Producto>(item);
                    await _ctx.Producto.AddAsync(Producto);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Producto creado con éxito" });
                }
                return Ok(new { success = true, mensaje = "Error al crear producto, intentelo de nuevo" });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPut("{Codigo}")]
        public async Task<IActionResult> Put(string Codigo, [FromBody] Producto_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Producto = _mapper.Map<T_Producto>(item);
                    Producto.Codigo = Codigo;
                    await _ctx.Producto.Update(Producto);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Producto editado con éxito" });
                }
                return Ok(new { success = true, mensaje = "Error al editar producto, intentelo de nuevo" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }
    }
}
